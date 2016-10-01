using Feeder.Common.Utils;
using Feeder.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Feeder.PivotApp.View
{
    public sealed partial class ArticlesControl : UserControl
    {
        public event ItemClickEventHandler ItemClicked;
        private FeedItemViewModel selectedItem;

        public ArticlesControl()
        {
            this.InitializeComponent();            
        }

        public void ScrollToSelectedItem(FeedItemViewModel model)
        {
            selectedItem = model;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var handler = ItemClicked;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        private void listViewFeeds_Loaded(object sender, RoutedEventArgs e)
        {
            if (selectedItem == null) return;

            foreach (var item in listViewFeeds.Items)
            {
                if (((FeedItemViewModel)item).ItemUri == selectedItem.ItemUri)
                {
                    listViewFeeds.ScrollIntoView(item, ScrollIntoViewAlignment.Leading);
                    break;
                }
            }
        }
    }
}
