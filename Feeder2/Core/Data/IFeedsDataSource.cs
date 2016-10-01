using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feeder.DataModel;
using System.Threading;

namespace Feeder.Common.Data
{
    public interface IFeedsDataSource
    {
        Task<FeedModel> GetFeedItemsAsync(string url);
        Task<FeedModel> GetFeedItemsAsync(string url, CancellationToken token);        
    }
}
