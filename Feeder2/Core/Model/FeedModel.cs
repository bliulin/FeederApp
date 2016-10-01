using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feeder.Common.Data;
using Feeder.Common.Factory;
using Feeder.Common.Model;
using Feeder.Common;
using Feeder.Common.Settings;
using System.Threading;

namespace Feeder.DataModel
{
    public class FeedModel
    {
        private List<FeedItemModel> mItems;

        public string Id { get; set; }

        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string ImageUri
        {
            get;
            set;
        }

        public List<FeedItemModel> Items
        {
            get
            {
                if (mItems == null)
                {
                    mItems = new List<FeedItemModel>();
                }
                return mItems;
            }
            set
            {
                mItems = value;
            }
        }

        public string Name
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public DateTime? LastUpdateTime
        {
            get;
            set;
        }

        public string ParentGroupName
        {
            get;
            set;
        }

        public async Task LoadData(CancellationToken token)
        {
            var feedSource = InstanceFactory.GetInstance<IFeedsDataSource>();
            var feedModel = await feedSource.GetFeedItemsAsync(Url, token);
            setupItems(feedModel);
        }

        public async Task LoadData()
        {
            var feedSource = InstanceFactory.GetInstance<IFeedsDataSource>();
            var feedModel = await feedSource.GetFeedItemsAsync(Url);
            setupItems(feedModel);
        }

        private void setupItems(FeedModel feedModel)
        {
            int articlesCount = feedModel.Items.Count;

            foreach (var article in feedModel.Items)
            {
                bool existsInList = Items.Exists(ik => ik.Id == article.Id);
                if (!existsInList)
                {
                    Items.Add(article);
                }
            }
            Items = Items.OrderByDescending(article => article.PublishDate).ToList();

            bool keepOnlyLatestArticles = InstanceFactory.GetInstance<IConfiguration>().KeepOnlyLatestArticles;
            if (keepOnlyLatestArticles && Items.Count > articlesCount)
            {
                Items.RemoveRange(articlesCount, Items.Count - articlesCount);
            }

            LastUpdateTime = DateTime.UtcNow;
        }
    }
}
