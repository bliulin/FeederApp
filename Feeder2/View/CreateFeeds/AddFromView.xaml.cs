using Feeder.Common.Factory;
using Feeder.Common.Repository;
using Feeder.PivotApp.Telemetry;
using Feeder.PivotApp.View.CreateFeeds;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Feeder.Common.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Feeder.PivotApp.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddFromView : Page
    {
        private string folderName;

        public AddFromView()
        {
            this.InitializeComponent();
        }

        private ITelemetry TelemetryClient
        {
            get
            {
                return InstanceFactory.GetInstance<ITelemetry>();
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                folderName = (string)e.Parameter;
            }
            await popularFeedsCtrl.InitializePopularFeeds();
        }

        #region Private methods
        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var tb = ((StackPanel)e.ClickedItem).Children[1] as TextBlock;

            if (tb.Text == "Search online for feeds")
            {
                Frame.Navigate(typeof(SearchForFeedsView), folderName);
            }
            else if (tb.Text == "Add feed manually")
            {
                var addFeedDialog = new FeedEditDialog(folderName);
                await addFeedDialog.ShowAsync();
            }
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

        private async void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var groups = popularFeedsCtrl.GetSelectedFeeds();
                var repo = InstanceFactory.GetInstance<IFeedRepository>();
                repo.MergeFolders(groups);
                trackEvents(groups);
                await repo.SaveAll();

                navigateBack();
            }
            catch (Exception ex)
            {
                TelemetryClient.TrackException(ex);
            }
        }

        private void trackEvents(FeedGroupModel[] groups)
        {
            var feeds = groups.SelectMany(g => g.Feeds).ToArray();
            foreach (var feed in feeds)
            {
                TelemetryClient.TrackFeedAddedFromList(feed);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            navigateBack();
        } 
        #endregion
    }
}
