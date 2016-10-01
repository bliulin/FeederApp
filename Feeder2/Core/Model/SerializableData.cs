using Feeder.Common.Model;
using Feeder.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feeder.PivotApp.Core.Model
{
    public class SerializableData
    {
        private List<FeedGroupModel> mFeedGroups = new List<FeedGroupModel>();
        private List<FeedItemModel> mSavedArticles = new List<FeedItemModel>();

        public List<FeedGroupModel> FeedGroups
        {
            get
            {
                return mFeedGroups;
            }
            set
            {
                mFeedGroups = value;
            }
        }

        public List<FeedItemModel> SavedArticles
        {
            get
            {
                return mSavedArticles;
            }
            set
            {
                mSavedArticles = value;
            }
        }        
    }
}
