using Feeder.Common.Factory;
using Feeder.Common.Repository;
using Feeder.Common.Utils;
using Feeder.Common.ViewModel;
using Feeder.PivotApp.Common;
using Feeder.PivotApp.Telemetry;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Feeder.PivotApp.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FeedItemPage : Page
    {
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly NavigationHelper navigationHelper;

        public FeedItemPage()
        {
            this.InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;
            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += navigationHelper_LoadState;
            navigationHelper.SaveState += navigationHelper_SaveState;
        }

        private ITelemetry TelemetryClient
        {
            get
            {
                return InstanceFactory.GetInstance<ITelemetry>();
            }
        }

        public FeedItemViewModel ViewModel { get; private set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataTransferManager dtManager = DataTransferManager.GetForCurrentView();
            dtManager.DataRequested += dtManager_DataRequested;

            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        private async void WebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (args.Uri != null)
            {
                args.Cancel = true;
                await Launcher.LaunchUriAsync(args.Uri);
            }
        }

        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            string modelId = e.NavigationParameter as string;
            var model = InstanceFactory.GetInstance<IFeedRepository>().FindFeedItem(modelId);
            ViewModel = FeedItemViewModel.Create(model);

            if (!ViewModel.IsRead)
            {
                ViewModel.IsRead = true;
                await saveData();
            }

            WebView.NavigateToString(ViewModel.Content);

            DataContext = ViewModel;
        }

        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        private async Task saveData()
        {
            try
            {
                var repo = InstanceFactory.GetInstance<IFeedRepository>();
                await repo.SaveAll();
            }
            catch (Exception ex)
            {
                TelemetryClient.TrackException(ex);
            }
        }

        private async void goToArticle(object sender, RoutedEventArgs e)
        {
            try
            {
                trackOpenArticleEvent(ViewModel.ItemUri);

                if (!string.IsNullOrWhiteSpace(ViewModel.ItemUri))
                {
                    await Launcher.LaunchUriAsync(new Uri(ViewModel.ItemUri));
                }
            }
            catch (Exception ex)
            {
                TelemetryClient.TrackException(ex);
            }
        }

        private void trackOpenArticleEvent(string uri)
        {
            try
            {
                var parameters = new Dictionary<string, string>();
                parameters.Add("url", uri);
                TelemetryClient.TrackEvent(Events.OPEN_ARTICLE, parameters);
            }
            catch (Exception ex)
            {
                TelemetryClient.TrackException(ex);
            }
        }

        private void btnShareLink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                trackShareArticleEvent();
                DataTransferManager.ShowShareUI();
            }
            catch (Exception ex)
            {
                TelemetryClient.TrackException(ex);
            }
        }

        private async void btnSaveArticle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                trackSavedArticleEvent();
                var repo = InstanceFactory.GetInstance<IFeedRepository>();
                await repo.SaveArticleAsync(ViewModel.Model);
                ViewModel.IsSaved = true;
            }
            catch (Exception ex)
            {
                TelemetryClient.TrackException(ex);
            }
        }

        private void dtManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            try
            {                
                e.Request.Data.Properties.Title = ViewModel.Title;
                e.Request.Data.Properties.Description = ViewModel.Title;
                e.Request.Data.SetWebLink(new Uri(ViewModel.ItemUri));
            }
            catch (Exception ex)
            {
                TelemetryClient.TrackException(ex);
            }
        }

        private void trackShareArticleEvent()
        {
            var properties = new Dictionary<string, string>();
            properties.Add("title", ViewModel.Title);
            properties.Add("url", ViewModel.ItemUri);
            TelemetryClient.TrackEvent(Events.SHARE_ARTICLE, properties);
        }

        private void trackSavedArticleEvent()
        {
            var properties = new Dictionary<string, string>();
            properties.Add("title", ViewModel.Title);
            properties.Add("url", ViewModel.ItemUri);
            TelemetryClient.TrackEvent(Events.SAVE_ARTICLE, properties);
        }
    }
}
