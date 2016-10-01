using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feeder.Common.Model;
using Feeder.DataModel;

namespace Feeder.Common.Repository
{
    public interface IFeedRepository
    {
        List<FeedGroupModel> FeedGroups
        {
            get;
        }

        List<FeedItemModel> SavedArticles { get; }

        Task<List<FeedGroupModel>> LoadFeedGroupsFromStorage();

        Task<List<FeedItemModel>> LoadSavedArticlesFromStorage();

        Task SaveFeed(FeedModel feedSource);

        Task SaveAll();

        Task SaveArticleAsync(FeedItemModel article);

        Task<bool> DeleteFeed(string id);

        Task DeleteFolder(string title);

        void MergeFolders(FeedGroupModel[] groups);

        IEnumerable<string> GetGroupNames();

        Task WriteContentToFile(string content);

        void Reset();

        FeedGroupModel FindGroup(string groupTitle);

        FeedModel FindFeed(string feedId);

        FeedItemModel FindFeedItem(string feedItemId);

        bool ContainsArticle(string groupName, string articleId);
    }
}
