using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feeder.Common.ViewModel;

namespace Feeder.PivotApp.Common
{
    class StateManager : IStateManager
    {
        private FeedItemViewModel mSelectedItem;

        public bool ReloadArticles
        {
            get; set;
        }

        public FeedItemViewModel SelectedItem
        {
            get
            {
                return mSelectedItem;
            }
            set
            {
                mSelectedItem = value;
            }
        }
    }
}
