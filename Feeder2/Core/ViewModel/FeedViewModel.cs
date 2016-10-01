using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Feeder.DataModel;
using System.Linq;
using Feeder.Common.Utils;
using System.Threading;

namespace Feeder.Common.ViewModel
{
    public class FeedViewModel : BaseListItem, IViewModel
    {
        #region Instance fields

        private ICommand mSettingsCommand;
        private ObservableCollection<FeedItemViewModel> mItems = new ObservableCollection<FeedItemViewModel>();
        private string mImageUri;

        #endregion

        #region Properties

        public override ICommand SettingsCommand
        {
            get
            {
                if (mSettingsCommand == null)
                {
                    mSettingsCommand = new RelayCommand(openSettings, () => true);
                }
                return mSettingsCommand;
            }
            set
            {
                mSettingsCommand = value;
            }
        }

        public string UrlAddress
        {
            get
            {
                return Model.Url;
            }
            set
            {
                Model.Url = value;
                OnNotifyPropertyChanged("UrlAddress");
            }
        }

        public string ParentFolderName
        {
            get;
            set;
        }

        public string Filter
        {
            get;
            set;
        }

        public FeedModel Model
        {
            get;
            set;
        }

        public string Title
        {
            get
            {
                return Model.Title;
            }
            set
            {
                Model.Title = value;
                OnNotifyPropertyChanged("Title");
            }
        }

        public ObservableCollection<FeedItemViewModel> Items
        {
            get
            {
                return mItems;
            }
            set
            {
                mItems = value;
                OnNotifyPropertyChanged("Items");
            }
        }

        public string ImageUri
        {
            get
            {
                return mImageUri;
            }
            set
            {
                mImageUri = value;
                OnNotifyPropertyChanged("ImageUri");
            }
        }

        #endregion

        #region >> IViewModel Members

        public void Initialize(object parameter)
        {
        }

        public Task<bool> OnLeaving(object parameter)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Public Methods

        public void UpdateSummary()
        {
            Summary = DateTimeUtils.GetElapsedTimeDescription(Model.LastUpdateTime);
        }

        public override async Task LoadData()
        {
            await Model.LoadData();
            UpdateUI();
        }

        public async Task LoadData(CancellationToken token)
        {
            await Model.LoadData(token);
        }

        public void UpdateUI()
        {
            UpdateSummary();
            fillItems();
        }

        public void DeleteItems()
        {
            Items.Clear();
            Model.Items.Clear();
        }

        public void MarkAllAsRead()
        {
            foreach (var item in Items)
            {
                item.IsRead = true;
            }
        }

        #endregion

        #region Private Methods

        private void fillItems()
        {
            Items.Clear();
            foreach (var item in Model.Items)
            {
                Items.Add(FeedItemViewModel.Create(item));
            }

            ItemsCount = (from article in Model.Items
                          where !article.IsRead
                          select article).Count();
        }

        private void openSettings()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Statics

        public static FeedViewModel Create(FeedModel model)
        {
            var viewModel = new FeedViewModel
            {
                Model = model
                , Name = model.Name
                , ItemsCount = (from article in model.Items
                               where !article.IsRead
                               select article).Count()
                , Summary = DateTimeUtils.GetElapsedTimeDescription(model.LastUpdateTime)
                , ParentFolderName = model.ParentGroupName      
                , ImageUri = model.ImageUri          
            };

            foreach (var item in model.Items)
            {
                var feedItemViewModel = FeedItemViewModel.Create(item);
                viewModel.Items.Add(feedItemViewModel);
            }

            return viewModel;
        }

        #endregion
    }
}