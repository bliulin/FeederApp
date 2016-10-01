using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Feeder.DataModel;
using Feeder.Common.Model;
using Newtonsoft.Json;
using Feeder.Common.Factory;
using Feeder.PivotApp.Telemetry;
using Feeder.PivotApp.Core.Model;

namespace Feeder.Common.Repository
{
    public class FileRepository : IFeedRepository
    {
        #region Instance fields

        private string mUserFilePath;
        private SemaphoreSlim mSemaphore = new SemaphoreSlim(1);
        private SerializableData mSerializableData = new SerializableData();
        private bool mDataLoaded;

        #endregion

        #region Properties

        public List<FeedGroupModel> FeedGroups
        {
            get
            {
                return mSerializableData.FeedGroups;
            }            
        }

        public List<FeedItemModel> SavedArticles
        {
            get
            {
                return mSerializableData.SavedArticles;
            }
            set
            {
                mSerializableData.SavedArticles = value;
            }
        }

        public string UserFilePath
        {
            get
            {
                return mUserFilePath ?? (mUserFilePath = getCurrentUserFilename());
            }
            set
            {
                mUserFilePath = value;
            }
        }

        private ITelemetry TelemetryClient
        {
            get
            {
                return InstanceFactory.GetInstance<ITelemetry>();
            }
        }

        #endregion

        #region Public methods
        public async Task SaveFeed(FeedModel feedSource)
        {
            if (string.IsNullOrWhiteSpace(feedSource.ParentGroupName))
            {
                feedSource.ParentGroupName = Constants.NEW_FOLDER;
            }
            var feedGroup = FeedGroups.FirstOrDefault(g => g.Title == feedSource.ParentGroupName);

            if (feedGroup == null)
            {
                feedGroup = new FeedGroupModel
                {
                    Title = feedSource.ParentGroupName,
                    Description = Constants.FOLDER_DESCRIPTION
                };

                FeedGroups.Add(feedGroup);
            }

            if (string.IsNullOrEmpty(feedSource.Id))
            {
                feedSource.Id = Guid.NewGuid().ToString();                
            }

            var existingFeed = feedGroup.Feeds.FirstOrDefault(feed => feed.Id == feedSource.Id);
            if (existingFeed == null)
            {
                feedGroup.Feeds.Add(feedSource);
                var otherGroups = FeedGroups.FindAll(g => g.Title != feedGroup.Title);

                foreach (var group in otherGroups)
                {
                    var feed = group.Feeds.FirstOrDefault(f => f.Id == feedSource.Id);
                    if (feed != null)
                    {
                        group.Feeds.Remove(feed);
                        break;
                    }
                }                
            }
            else
            {
                existingFeed = feedSource;
            }

            await saveData();
        }

        public async Task SaveAll()
        {
            await saveData();
        }

        public async Task<bool> DeleteFeed(string id)
        {
            try
            {
                await mSemaphore.WaitAsync();

                foreach (var group in FeedGroups)
                {
                    var feed = group.Feeds.FirstOrDefault(f => f.Id == id);
                    if (feed != null)
                    {
                        group.Feeds.Remove(feed);                        
                        return true;
                    }
                }

                return false;
            }
            finally
            {
                mSemaphore.Release();
                await saveData();
            }
        }

        public async Task DeleteFolder(string title)
        {
            var toDelete = FeedGroups.FirstOrDefault(g => g.Title == title);
            FeedGroups.Remove(toDelete);
            await saveData();
        }

        public async Task<List<FeedGroupModel>> LoadFeedGroupsFromStorage()
        {
            if (mDataLoaded)
            {
                return mSerializableData.FeedGroups;
            }

            await loadDataFromStorage();
            mDataLoaded = true;
            return mSerializableData.FeedGroups;
        }

        public async Task<List<FeedItemModel>> LoadSavedArticlesFromStorage()
        {
            if (mDataLoaded)
            {
                return mSerializableData.SavedArticles;
            }

            await loadDataFromStorage();
            mDataLoaded = true;
            return mSerializableData.SavedArticles;
        }

        public async Task SaveArticleAsync(FeedItemModel article)
        {
            bool exists = mSerializableData.SavedArticles.Exists(a => a.ItemUri == article.ItemUri);
            if (!exists)
            {
                mSerializableData.SavedArticles.Insert(0, article);
            }
            await saveData();
        }

        public IEnumerable<string> GetGroupNames()
        {
            return FeedGroups.Select(group => group.Title);
        }

        public FeedGroupModel FindGroup(string groupTitle)
        {
            var group = FeedGroups.FirstOrDefault(g => string.CompareOrdinal(g.Title, groupTitle) == 0);
            return group;
        }

        public FeedModel FindFeed(string feedId)
        {
            foreach (var group in FeedGroups)
            {
                var feed = group.Feeds.FirstOrDefault(f => f.Id == feedId);
                if (feed != null)
                {
                    return feed;
                }
            }
            return null;
        }

        public FeedItemModel FindFeedItem(string feedItemId)
        {
            FeedItemModel wanted = null;

            foreach (var group in FeedGroups)
            {
                foreach (var feed in group.Feeds)
                {
                    wanted = feed.Items.FirstOrDefault(item => item.Id == feedItemId);
                    if (wanted != null) break;
                }
                if (wanted != null) break;
            }
            return wanted ?? SavedArticles.FirstOrDefault(article => article.Id == feedItemId);
        }

        public void Reset()
        {
            mSerializableData = null;
        }

        public async Task WriteContentToFile(string content)
        {
            try
            {
                StorageFile file = await getFeedSourceStorageFile();
                await FileIO.WriteTextAsync(file, content);
            }
            catch (Exception ex)
            {                
                TelemetryClient.TrackException(ex);
            }
        }

        public void MergeFolders(FeedGroupModel[] groups)
        {
            foreach (var group in groups)
            {
                var existingGroup = FeedGroups.FirstOrDefault(g => g.Title == group.Title);
                if (existingGroup == default(FeedGroupModel))
                {
                    FeedGroups.Add(group);                    
                }
                else
                {
                    existingGroup.Feeds.AddRange(group.Feeds);
                }
            }            
        }

        #endregion

        #region Private methods        

        private async Task loadDataFromStorage()
        {
            var storageFile = await getFeedSourceStorageFile();
            string content = await FileIO.ReadTextAsync(storageFile);
            if (!string.IsNullOrEmpty(content))
            {
                mSerializableData = JsonConvert.DeserializeObject<SerializableData>(content);
            }
        }        

        private string getCurrentUserFilename()
        {
            return Constants.FEEDS_FILENAME;
        }

        private async Task<StorageFile> getFeedSourceStorageFile()
        {
            StorageFolder folder = ApplicationData.Current.RoamingFolder;

            StorageFile storage = null;
            string fileName = UserFilePath;

            try
            {
                storage = await folder.GetFileAsync(fileName);
            }
            catch (FileNotFoundException)
            {
            }

            try
            {
                if (storage == null)
                {
                    storage = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                }
            }
            catch (Exception ex)
            {
                TelemetryClient.TrackException(ex);
            }

            return storage;
        }

        private async Task saveData()
        {
            try
            {
                string content = JsonConvert.SerializeObject(mSerializableData);
                await WriteContentToFile(content);
            }
            catch (Exception ex)
            {
                TelemetryClient.TrackException(ex);
            }
        }

        public bool ContainsArticle(string groupName, string articleId)
        {
            var group = FeedGroups.FirstOrDefault(g => g.Title == groupName);
            if (group == null)
            {
                throw new Exception("groupName does not exist: " + groupName);
            }

            return group.Feeds.Exists(feed => feed.Items.Exists(article => article.Id == articleId));
        }

        #endregion
    }
}
