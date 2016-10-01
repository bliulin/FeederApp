using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feeder.Common.Factory;
using Feeder.Common.Repository;

namespace Feeder.Common.ViewModel
{
    public class FeederListViewModel<T> : ObservableCollection<T> where T : BaseListItem
    {
        public void LoadData()
        {
            foreach (var item in this)
            {
                item.LoadData();
            }
        }        
    }
}
