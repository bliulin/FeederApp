using Feeder.Common.ViewModel;
using Feeder.PivotApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feeder.PivotApp.DataModel
{
    static class PopularFeedsModel
    {
        public static ObservableDictionary GetPopularFeeds()
        {
            ObservableDictionary defaultViewModel = new ObservableDictionary();
            return defaultViewModel;
        }

        private static List<SearchResultViewModel> GetNewsFeeds()
        {
            return null;
        }
    }
}
