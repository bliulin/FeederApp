using Feeder.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feeder.Common.ViewModel
{
    public class SearchResultViewModel : BaseViewModel
    {
        bool? mIsSelected;

        public bool Exists { get; set; }

        public bool? IsSelected
        {
            get
            {
                return mIsSelected;
            }
            set
            {
                mIsSelected = value;
                OnNotifyPropertyChanged("IsSelected");
            }
        }

        public string ImageUri
        {
            get;
            set;
        }

        public string Title
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }

        public FeedModel Model { get; set; }
    }
}
