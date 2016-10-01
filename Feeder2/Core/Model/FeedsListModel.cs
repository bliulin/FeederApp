using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feeder.Common.Model
{
    //TODO: use this class to support Feed Groups and Feeds on the root level.
    public class FeedsListModel
    {
        public List<FeedGroupModel> FeedGroupModels
        {
            get;
            set;
        }        
    }
}
