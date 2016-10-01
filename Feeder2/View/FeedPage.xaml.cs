// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

using System;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Feeder.Common.ViewModel;
using Feeder.DataModel;
using Feeder.PivotApp.Common;
using System.Threading.Tasks;
using Feeder.Common.Factory;
using Feeder.Common.Repository;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Feeder.PivotApp.Telemetry;
using System.Collections.Generic;
using Feeder.PivotApp.Settings;
using Feeder.Common.Settings;
using System.Text;

namespace Feeder.PivotApp.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FeedPage : Page
    {
        #region Instance fields

        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly NavigationHelper navigationHelper;
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        #endregion

        #region Properties     

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get
            {
                return navigationHelper;
            }
        }

        public FeedViewModel FeedViewModel
        {
            get;
            set;
        }

        private ITelemetry TelemetryClient
        {
            get
            {
                return InstanceFactory.GetInstance<ITelemetry>();
            }
        }

        private IConfiguration Config
        {
            get
            {
                return InstanceFactory.GetInstance<IConfiguration>();
            }
        }

        private IStateManager StateManager
        {
            get
            {
                return InstanceFactory.GetInstance<IStateManager>();
            }
        }

        #endregion

        #region Constructors

        public FeedPage()
        {
            InitializeComponent();

            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += navigationHelper_LoadState;
            navigationHelper.SaveState += navigationHelper_SaveState;
        }

        #endregion

        #region Protected Methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        #region Private Methods

        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            try
            {
                string modelId = e.NavigationParameter as string;
                var model = InstanceFactory.GetInstance<IFeedRepository>().FindFeed(modelId);
                FeedViewModel = FeedViewModel.Create(model);
                DataContext = FeedViewModel;

                if (canUpdate())
                {
                    await loadFeedsFromWildWildWeb();
                    await saveFeedData();
                }
                else
                {
                    articlesControl.ScrollToSelectedItem(StateManager.SelectedItem);
                }
                StateManager.ReloadArticles = false;
            }
            catch (Exception ex)
            {
                trackFeedLoadException(ex);
            }
        }

        private void trackFeedLoadException(Exception ex)
        {
            StringBuilder sb = new StringBuilder("Failed to load feed data. Feed state at the exception moment: ");
            sb.AppendLine();
            sb.AppendLine($"Feed title: {FeedViewModel.Title}");
            sb.AppendLine($"Feed URL: {FeedViewModel.UrlAddress}");
            sb.AppendLine($"Parent folder: {FeedViewModel.ParentFolderName}");
            sb.AppendLine($"Items count: {FeedViewModel.ItemsCount}");
            sb.AppendLine($"Exception message: {ex.Message}");

            var wrapperException = new Exception(sb.ToString(), ex);
            TelemetryClient.TrackException(ex);
        }

        private bool canUpdate()
        {
            return (Config.AutoUpdate && StateManager.ReloadArticles) || FeedViewModel.Items.Count == 0;
        }

        private async Task showLoadingIndicator()
        {
            var statusBar = StatusBar.GetForCurrentView();
            await statusBar.ProgressIndicator.ShowAsync();
        }

        private async Task hideLoadingIndicator()
        {
            var statusBar = StatusBar.GetForCurrentView();
            await statusBar.ProgressIndicator.HideAsync();
        }

        private async Task loadFeedsFromWildWildWeb()
        {
            await showLoadingIndicator();

            try
            {
                trackFeedUpdatedEvent(FeedViewModel.Model);                
                await FeedViewModel.LoadData();
            }
            catch (Exception)
            {
                var dialog = new MessageDialog("There was a problem updating the feed.");
                dialog.Commands.Add(new UICommand("Close"));
                await dialog.ShowAsync();
                throw;
            }
            finally
            {
                await hideLoadingIndicator();
            }
        }        

        private async Task saveFeedData()
        {
            try
            {
                var repo = InstanceFactory.GetInstance<IFeedRepository>();
                await repo.SaveFeed(FeedViewModel.Model);
            }
            catch (Exception ex)
            {
                TelemetryClient.TrackException(ex);
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as FeedItemViewModel;

            if (item == null) return;
            else
            {
                StateManager.SelectedItem = item;
            }

            Frame.Navigate(typeof(FeedItemPage), item.Model.Id);
        }
        
        private async void btnUpdate_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                await loadFeedsFromWildWildWeb();
                await saveFeedData();
            }
            catch (Exception ex)
            {                
                trackFeedLoadException(ex);
            }
        }

        private async void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new MessageDialog("Remove the articles from this list?");
                dialog.Commands.Add(new UICommand("Yes", deleteFeedItems));
                dialog.Commands.Add(new UICommand("Cancel"));
                dialog.DefaultCommandIndex = 0;
                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                TelemetryClient.TrackException(ex);
            }
        }

        private async void deleteFeedItems(IUICommand command)
        {
            TelemetryClient.TrackFeedDeleted(FeedViewModel.Model);
            FeedViewModel.DeleteItems();
            await saveFeedData();
        }

        private void btnMarkAllAsRead_Click(object sender, RoutedEventArgs e)
        {
            FeedViewModel.MarkAllAsRead();
        }

        private void trackFeedUpdatedEvent(FeedModel feed)
        {
            TelemetryClient.TrackFeedUpdated(feed);
        }

        #endregion        
    }
}