using Feeder.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feeder.Common.Services
{
    public interface IFeedSearcher
    {
        Task<FeedModel[]> SearchAsync(string query);
    }
}
