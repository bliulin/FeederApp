using System.Threading.Tasks;
using Feeder.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Feeder.DataModel;
using Feeder.PivotApp.Common;
using Feeder.PivotApp.Telemetry;
using Feeder.Common.Factory;
using System.Text.RegularExpressions;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Feeder.PivotApp.View
{
    public sealed partial class FeedEditDialog : ContentDialog
    {
        private FeedModel mModel;

        public FeedEditDialog() : this(string.Empty)
        {
        }

        public FeedEditDialog(string folderName)
        {
            this.InitializeComponent();

            ViewModel = new FeedEditViewModel();
            ViewModel.LoadData();

            if (!string.IsNullOrEmpty(folderName))
            {
                ViewModel.SelectedFolderName = folderName;
            }

            this.DataContext = ViewModel;
        }

        public FeedEditDialog(FeedModel model)
        {
            this.InitializeComponent();

            mModel = model;
            var viewModel = new FeedEditViewModel
            {
                FeedName = model.Name,
                UrlAddress = model.Url,
                SelectedFolderName = model.ParentGroupName
            };

            ViewModel = viewModel;
            ViewModel.LoadData();

            this.DataContext = ViewModel;
        }

        private ITelemetry TelemetryClient
        {
            get
            {
                return InstanceFactory.GetInstance<ITelemetry>();
            }
        }

        public FeedEditViewModel ViewModel { get; set; }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {
                clearErrorMessage();

                string message;
                if (validate(out message))
                {
                    await ViewModel.SaveData(mModel);
                    TelemetryClient.TrackFeedAddedManuallyEvent(mModel);
                }
                else
                {
                    displayErrorMessage(message);
                    args.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                TelemetryClient.TrackException(ex);
            }
        }

        private void displayErrorMessage(string message)
        {
            error.Text = message;
        }

        private void clearErrorMessage()
        {
            error.Text = "";
        }

        private bool validate(out string message)
        {            
            message = "";
            return true;

            if (string.IsNullOrEmpty(ViewModel.UrlAddress))
            {
                message = "URL is empty, please provide a value";
                return false;
            }

            const string uriPattern = @"^(https?|ftp)://[^\s/$.?#].[^\s]*$";

            if (!Regex.IsMatch(ViewModel.UrlAddress, uriPattern))
            {
                message = "URL is not a valid value";
                return false;
            }

            if (string.IsNullOrEmpty(ViewModel.FeedName))
            {
                message = "Feed name is empty, please provide a value";
                return false;
            }

            if (string.IsNullOrEmpty(ViewModel.NewFolderName) && 
                (comboBoxFolder.SelectedIndex == 0 || comboBoxFolder.SelectedIndex == -1))
            {
                message = "Please select a folder or type a new folder name";
                return false;
            }

            return true;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
