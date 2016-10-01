using Feeder.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feeder.PivotApp.Common
{
    interface IStateManager
    {
        bool ReloadArticles { get; set; }

        FeedItemViewModel SelectedItem
        {
            get; set;
        }
    }
}
