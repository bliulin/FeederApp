// The Pivot Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Feeder.Common.Factory;
using Feeder.Common.Repository;
using Feeder.Common.ViewModel;
using Feeder.PivotApp.Common;
using Feeder.PivotApp.View;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Input;
using Windows.UI.Popups;
using System.Linq;
using Feeder.PivotApp.Telemetry;
using Feeder.PivotApp.Settings;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Xml.Linq;
using System.Xml;
using System.Reflection;
using Windows.ApplicationModel;
using Feeder.PivotApp.View.About;
using Feeder.PivotApp.View.Search;
using Feeder.Common.Data;
using System.Collections.Generic;

namespace Feeder.PivotApp
{
    public sealed partial class PivotPage : Page
    {
        #region Constants

        private const string FeedGroupName = "FeedGroups";
        private const string Items = "Items";

        #endregion

        #region Instance fields

        private readonly ObservableDictionary mDefaultViewModel = new ObservableDictionary();
        private readonly NavigationHelper mNavigationHelper;
        private readonly ResourceLoader mResourceLoader = ResourceLoader.GetForCurrentView("Resources");
        private FeederListViewModel<FolderViewModel> mFeedGroups = new FeederListViewModel<FolderViewModel>();
        private FolderViewModel mSelectedGroup;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get
            {
                return mNavigationHelper;
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
                return mDefaultViewModel;
            }
        }

        public FeederListViewModel<FolderViewModel> FeedGroups
        {
            get
            {
                return mFeedGroups;
            }
            set
            {
                mFeedGroups = value;
            }
        }

        public IFeedRepository ModelRepository { get; private set; }

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

        public PivotPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;

            mNavigationHelper = new NavigationHelper(this);
            mNavigationHelper.LoadState += NavigationHelper_LoadState;
            mNavigationHelper.SaveState += NavigationHelper_SaveState;
        }

        #endregion

        #region Protected Methods

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
            mNavigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            mNavigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>.
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            try
            {                
                if (ModelRepository == null)
                {
                    ModelRepository = InstanceFactory.GetInstance<IFeedRepository>();
                    await ModelRepository.LoadFeedGroupsFromStorage();
                }

                loadFeedGroupsIntoViewModel();
                DefaultViewModel[FeedGroupName] = FeedGroups;
                var articles = await loadArticles(pivot.SelectedIndex);
                DefaultViewModel[Items] = articles;

                articleCtrlAll.ScrollToSelectedItem(StateManager.SelectedItem);
                articleCtrlUnread.ScrollToSelectedItem(StateManager.SelectedItem);
            }
            catch (Exception ex)
            {
                TelemetryClient.TrackException(ex);                
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache. Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/>.</param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }

        /// <summary>
        /// Adds an item to the list when the app bar button is clicked.
        /// </summary>
        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            showAddFromView();
        }

        private void showAddFromView()
        {
            Frame.Navigate(typeof (AddFromView));
        }

        private void loadFeedGroupsIntoViewModel()
        {
            FeedGroups.Clear();
            foreach (var feedGroupModel in ModelRepository.FeedGroups)
            {
                FeedGroups.Add(FolderViewModel.Create(feedGroupModel));
            }            
        }

        /// <summary>
        /// Invoked when an item within a section is clicked.
        /// </summary>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var viewModelItem = e.ClickedItem as FolderViewModel;
            if (viewModelItem != null)
            {
                Frame.Navigate(typeof (FeedsPage), viewModelItem.Model.Title);
            }
        }

        private async void MenuFlyoutItemDelete_Click(object sender, RoutedEventArgs e)
        {
            var dialog2 = new MessageDialog("Delete this folder and all its contents?");
            dialog2.Commands.Add(new UICommand("Delete", deleteFolder));
            dialog2.Commands.Add(new UICommand("Cancel"));
            dialog2.CancelCommandIndex = 1;
            await dialog2.ShowAsync();
        }

        private async void deleteFolder(IUICommand command)
        {
            if (mSelectedGroup == null) return;

            FeedGroups.Remove(mSelectedGroup);
            await ModelRepository.DeleteFolder(mSelectedGroup.Model.Title);
        }

        private void StackPanel_Holding(object sender, Windows.UI.Xaml.Input.HoldingRoutedEventArgs args)
        {
            // this event is fired multiple times. We do not want to show the menu twice
            if (args.HoldingState != HoldingState.Started) return;

            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            var selectedItem = element.DataContext as FolderViewModel;
            if (selectedItem != null)
            {
                mSelectedGroup = selectedItem;
            }

            // If the menu was attached properly, we just need to call this handy method
            FlyoutBase.ShowAttachedFlyout(element);
        }

        private void SettingsAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        private void MenuFlyoutItemRename_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuFlyoutItem;
            var vm = (FolderViewModel)menuItem.DataContext;

            flag = false;
            searchVisualTree(listFeedGroups, vm.Name);
        }

        private bool flag = false;
        private async void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb;

            if (!flag)
            {
                tb = sender as TextBox;
                tb.Focus(FocusState.Programmatic);
                tb.SelectAll();
                flag = true;
            }
            else
            {
                tb = sender as TextBox;
                if (tb != null)
                {
                    var vm = tb.DataContext as FolderViewModel;
                    if (vm != null)
                    {
                        vm.Name = tb.Text;
                        await saveChanges(vm);
                    }
                }

                var textBox = sender as TextBox;
                textBox.Visibility = Visibility.Collapsed;

                var stackPanel = textBox.Parent as StackPanel;
                var textBlock = stackPanel.Children.First(c => c is TextBlock);
                textBlock.Visibility = Visibility.Visible;
            }
        }

        private void searchVisualTree(DependencyObject targetElement, string name)
        {
            var count = VisualTreeHelper.GetChildrenCount(targetElement);
            if (count == 0)
                return;

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(targetElement, i);
                if (child is TextBlock)
                {
                    TextBlock targetItem = (TextBlock)child;

                    if (targetItem.Text == name)
                    {
                        targetItem.Visibility = Visibility.Collapsed;

                        var stackPanel = targetItem.Parent as StackPanel;
                        var textBoxName = stackPanel.Children.First(c => c is TextBox) as TextBox;
                        textBoxName.Visibility = Visibility.Visible;

                        textBoxName.Focus(FocusState.Pointer);

                        return;
                    }
                }
                else
                {
                    searchVisualTree(child, name);
                }
            }
        }

        private async Task saveChanges(FolderViewModel vm)
        {
            if (vm != null)
            {
                foreach (var item in vm.Model.Feeds)
                {
                    item.ParentGroupName = vm.Model.Title;
                }
                var repo = InstanceFactory.GetInstance<IFeedRepository>();
                await repo.SaveAll();
            }
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AboutPage));
        }

        private void searchAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SearchPage));
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

        private async void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var articles = await loadArticles(pivot.SelectedIndex);
            DefaultViewModel[Items] = articles;
        }

        private async Task<List<FeedItemViewModel>> loadArticles(int index)
        {
            var articleFilter = new ArticleFilter();

            switch (index)
            {
                case 1: return articleFilter.GetAllArticles();
                case 2: return articleFilter.GetUnreadArticles();
                case 3: return await articleFilter.GetSavedArticles();
                default: return new List<FeedItemViewModel>();
            }            
        }        

        #endregion


    }
}