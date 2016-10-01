// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using System;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Feeder.Common.Factory;
using Feeder.Common.Model;
using Feeder.Common.Repository;
using Feeder.Common.ViewModel;
using Feeder.PivotApp.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Input;
using Windows.UI.Popups;
using Feeder.Common.Data;
using Windows.UI.ViewManagement;
using System.Threading.Tasks;
using Feeder.PivotApp.Telemetry;
using System.Collections.Generic;
using System.Threading;
using System.Net.Http;
using Feeder.PivotApp.View.Search;
using Feeder.PivotApp.Settings;
using Feeder.PivotApp.View.About;

namespace Feeder.PivotApp.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FeedsPage : Page
    {
        #region Constants

        private const string Items = "Items";

        #endregion

        #region Instance fields

        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly NavigationHelper navigationHelper;
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");
        private FeedViewModel mSelectedFeed;
        private FeedGroupModel mCurrentModel;
        private CancellationTokenSource cts;

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

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get
            {
                return defaultViewModel;
            }
        }

        public FeederListViewModel<FeedViewModel> Feeds
        {
            get;
            set;
        }

        private IStateManager StateManager
        {
            get
            {
                return InstanceFactory.GetInstance<IStateManager>();
            }
        }

        private ITelemetry TelemetryClient
        {
            get
            {
                return InstanceFactory.GetInstance<ITelemetry>();
            }
        }

        #endregion

        #region Constructors

        public FeedsPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;
            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += navigationHelper_LoadState;
            navigationHelper.SaveState += navigationHelper_SaveState;
            navigationHelper.OnNavigatingBack += navigationHelper_OnNavigatingBack;
        }        

        #endregion

        #region Protected Methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        #region Private Methods

        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {            
        }

        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var sm = InstanceFactory.GetInstance<IStateManager>();
            sm.ReloadArticles = true;

            string groupTitle = e.NavigationParameter as string;
            DefaultViewModel["GroupTitle"] = groupTitle;
            mCurrentModel = InstanceFactory.GetInstance<IFeedRepository>().FindGroup(groupTitle);

            var articles = await loadArticles(pivot.SelectedIndex);
            DefaultViewModel[Items] = articles;

            updateUI();

            articleCtrlAll.ScrollToSelectedItem(StateManager.SelectedItem);
            articleCtrlUnread.ScrollToSelectedItem(StateManager.SelectedItem);
        }

        private async Task<List<FeedItemViewModel>> loadArticles(int index)
        {
            var articleFilter = new ArticleFilter();

            switch (index)
            {
                case 1: return articleFilter.GetAllArticles(mCurrentModel);
                case 2: return articleFilter.GetUnreadArticles(mCurrentModel);
                case 3: return await articleFilter.GetSavedArticles(mCurrentModel);
                default: return new List<FeedItemViewModel>();
            }
        }

        private void navigationHelper_OnNavigatingBack(object sender, EventArgs e)
        {
            if (cts != null)
            {
                cts.Cancel();
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var feedViewModel = e.ClickedItem as FeedViewModel;
            if (feedViewModel != null)
            {
                Frame.Navigate(typeof (FeedPage), feedViewModel.Model.Id);
            }
        }       
        
        private async void MenuFlyoutItem_DeleteClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var dialog = new MessageDialog("Delete this feed?");
            dialog.Commands.Add(new UICommand("Delete", deleteFeed));
            dialog.Commands.Add(new UICommand("Cancel"));

            dialog.CancelCommandIndex = 1;
            await dialog.ShowAsync();
        }

        private async void MenuFlyoutItem_EditClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var viewModel = new FeedEditViewModel();
            viewModel.FeedName = mSelectedFeed.Name;
            viewModel.UrlAddress = mSelectedFeed.UrlAddress;
            viewModel.SelectedFolderName = mSelectedFeed.ParentFolderName;
            var feedSettings = new FeedEditDialog(mSelectedFeed.Model);
            feedSettings.Closed += FeedSettings_Closed;
            await feedSettings.ShowAsync();
        }

        private void updateUI()
        {
            var feedGroup = mCurrentModel;
            if (feedGroup == null) return;

            Feeds = new FeederListViewModel<FeedViewModel>();
            foreach (var feedModel in feedGroup.Feeds)
            {
                var viewModel = FeedViewModel.Create(feedModel);
                viewModel.UpdateSummary();
                Feeds.Add(viewModel);
            }
            
            DefaultViewModel["FirstGroup"] = Feeds;
        }

        private void FeedSettings_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            updateUI();
        }

        private void deleteFeed(IUICommand command)
        {
            if (mSelectedFeed == null) return;

            Feeds.Remove(mSelectedFeed);

            var repository = InstanceFactory.GetInstance<IFeedRepository>();
            repository.DeleteFeed(mSelectedFeed.Model.Id);
        }

        private void StackPanel_Holding(object sender, Windows.UI.Xaml.Input.HoldingRoutedEventArgs args)
        {
            // this event is fired multiple times. We do not want to show the menu twice
            if (args.HoldingState != HoldingState.Started) return;

            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            var selectedItem = element.DataContext as FeedViewModel;
            if (selectedItem != null)
            {
                mSelectedFeed = selectedItem;
            }

            // If the menu was attached properly, we just need to call this handy method
            FlyoutBase.ShowAttachedFlyout(element);
        }

        private void ArticlesControl_ItemClicked(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as FeedItemViewModel;
            if (item == null) return;
            else
            {
                if (pivot.SelectedIndex == 2)
                {
                    // if the unread pivot is selected, find the next item in the list 
                    // (the selected one will not be in the list when navigating back).
                    ListView list = (ListView)sender;

                    int nextIndex = 0;
                    for (int i = 0; i < list.Items.Count; i++)
                    {
                        if (((FeedItemViewModel)list.Items[i]).ItemUri == item.ItemUri)
                        {
                            if (i + 1 < list.Items.Count)
                            {
                                nextIndex = i + 1;
                            }
                        }
                    }

                    StateManager.SelectedItem = list.Items[nextIndex] as FeedItemViewModel;
                }
                else
                {
                    StateManager.SelectedItem = item;
                }
            }

            Frame.Navigate(typeof(FeedItemPage), item.Model.Id);
        }

        private async void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var articles = await loadArticles(pivot.SelectedIndex);
            DefaultViewModel[Items] = articles;
        }        

        #endregion

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

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            await showLoadingIndicator();

            var tasks = new List<Task>();
            cts = new CancellationTokenSource();
            try
            {                
                foreach (var feed in Feeds)
                {
                    var t = Task
                        .Run(async () =>
                        {
                            try
                            {                                
                                await feed.LoadData(cts.Token).ConfigureAwait(false);
                            }
                            catch (OperationCanceledException)
                            {
                            }
                            catch (Exception ex)
                            {
                                TelemetryClient.TrackException(ex);
                            }
                        })
                        .ContinueWith(tk => this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, feed.UpdateUI));

                    tasks.Add(t);
                }
                await Task.WhenAll(tasks.ToArray());
                await saveData();
            }            
            finally
            {
                await hideLoadingIndicator();                
            }
        }

        static async Task<string> getData(string url, CancellationToken ct)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url, ct);
            string content = await response.Content.ReadAsStringAsync();
            return content;
        }

        private async Task saveData()
        {
            var repo = InstanceFactory.GetInstance<IFeedRepository>();
            try
            {
                await repo.SaveAll();
            }
            catch (Exception ex)
            {
                TelemetryClient.TrackException(ex);
            }
        }

        private void addAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddFromView), DefaultViewModel["GroupTitle"]);
        }

        private void searchAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SearchPage));
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AboutPage));
        }

        private void settingsAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }
    }
}