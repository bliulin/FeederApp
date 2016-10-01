using Feeder.Common.Factory;
using Feeder.Common.Repository;
using Feeder.Common.Services;
using Feeder.Common.ViewModel;
using Feeder.DataModel;
using Feeder.PivotApp.Common;
using Feeder.PivotApp.Telemetry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Feeder.PivotApp.View.CreateFeeds
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchForFeedsView : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private string folderName;

        public SearchForFeedsView()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        public IFeedSearcher FeedSearcher
        {
            get
            {
                return InstanceFactory.GetInstance<IFeedSearcher>();
            }
        }

        //public ILogger Logger
        //{
        //    get
        //    {
        //        return LogManagerFactory.DefaultLogManager.GetLogger<App>();
        //    }
        //}

        private ITelemetry TelemetryClient
        {
            get
            {
                return InstanceFactory.GetInstance<ITelemetry>();
            }
        }

        public ObservableCollection<SearchResultViewModel> Feeds { get; set; }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            folderName = (string)e.NavigationParameter;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TelemetryClient.TrackSearchEvent(txtSearch.Text);

                Feeds = new ObservableCollection<SearchResultViewModel>();

                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    var modelList = await FeedSearcher.SearchAsync(txtSearch.Text);
                    foreach (var model in modelList)
                    {
                        var viewModel = new SearchResultViewModel()
                        {
                            Model = model,
                            Title = model.Title,
                            Description = model.Description,
                            ImageUri = model.ImageUri
                        };
                        if (feedExists(model.Url))
                        {
                            viewModel.Exists = true;
                            viewModel.IsSelected = true;
                        }

                        Feeds.Add(viewModel);
                    }
                }

                DefaultViewModel["Feeds"] = Feeds;                
            }
            catch (Exception ex)
            {
                var exception = new Exception($"Error searching for {txtSearch.Text}. Message: {ex.Message}", ex);
                TelemetryClient.TrackException(exception);
            }
        }

        private bool feedExists(string url)
        {
            var repo = InstanceFactory.GetInstance<IFeedRepository>();
            bool exists = false;
            foreach (var group in repo.FeedGroups)
            {
                exists = group.Feeds.Any(feed => feed.Url.Equals(url, StringComparison.Ordinal));
                if (exists)
                {
                    return true;
                }
            }
            return false;
        }

        private async void btnAddFeeds_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var repo = InstanceFactory.GetInstance<IFeedRepository>();
                foreach (var item in Feeds)
                {
                    if (!item.Exists && item.IsSelected.HasValue && item.IsSelected.Value)
                    {
                        item.Model.ParentGroupName = folderName;
                        await repo.SaveFeed(item.Model);
                        TelemetryClient.TrackFeedAddedFromSearchEvent(item.Model);
                    }
                }
                navigateToTriggerPage();
            }
            catch (Exception ex)
            {
                TelemetryClient.TrackException(ex);
            }
        }

        private void navigateToTriggerPage()
        {
            var pageType = string.IsNullOrEmpty(folderName) ? typeof(PivotPage) : typeof(FeedsPage);
            Frame.Navigate(pageType, folderName);
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as SearchResultViewModel;
            item.IsSelected = item.IsSelected.HasValue ? !item.IsSelected : true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            navigateBack();
        }

        private void navigateBack()
        {
            Frame frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                return;
            }

            if (frame.CanGoBack)
            {
                frame.GoBack();
            }
        }
    }
}
