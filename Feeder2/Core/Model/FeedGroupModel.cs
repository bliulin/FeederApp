using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feeder.DataModel;

namespace Feeder.Common.Model
{
    public class FeedGroupModel
    {
        private List<FeedModel> mFeeds = new List<FeedModel>();

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

        public List<FeedModel> Feeds
        {
            get
            {
                return mFeeds;
            }
            set
            {
                mFeeds = value;
            }
        }
    }
}
