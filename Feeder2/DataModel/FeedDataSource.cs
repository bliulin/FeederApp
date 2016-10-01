using Feeder.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feeder.PivotApp.DataModel
{
    public class FeedDataSource
    {
        public FeederListViewModel<FolderViewModel> Folders
        {
            get;
            set;
        }
    }
}
