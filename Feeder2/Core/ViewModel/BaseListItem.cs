using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Feeder.Common.ViewModel
{
    public abstract class BaseListItem : BaseViewModel, INotifyPropertyChanged, INotifyCollectionChanged
    {
        private string mSummary;
        private string mName;
        private int? mItemsCount;

        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;                
                OnNotifyPropertyChanged("Name");
            }
        }

        public string Summary
        {
            get
            {
                return mSummary;
            }
            set
            {
                mSummary = value;
                OnNotifyPropertyChanged("Summary");
            }
        }

        public int? ItemsCount
        {
            get
            {
                return mItemsCount;
            }
            set
            {
                mItemsCount = value;
                OnNotifyPropertyChanged("ItemsCount");
            }
        }

        public virtual ICommand SettingsCommand
        {
            get;
            set;
        }

        public abstract Task LoadData();

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
