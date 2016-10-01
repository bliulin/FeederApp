using Feeder.Common.Factory;
using Feeder.Common.Model;
using Feeder.Common.Repository;
using Feeder.Common.ViewModel;
using Feeder.DataModel;
using Feeder.PivotApp.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class PopularFeedsControl : UserControl
    {
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private FeedGroupModel[] groups;

        public PopularFeedsControl()
        {
            this.InitializeComponent();
        }        

        public ObservableDictionary Feeds { get { return defaultViewModel; } }

        public async Task InitializePopularFeeds()
        {                        
            string jsonPopularFeeds = await readPopularFeeds();
            groups = JsonConvert.DeserializeObject<FeedGroupModel[]>(jsonPopularFeeds);
            foreach (var group in groups)
            {
                var feeds = new ObservableCollection<SearchResultViewModel>();
                foreach (var feedModel in group.Feeds)
                {
                    var searchVm = createViewModel(feedModel);
                    feeds.Add(searchVm);
                }
                Feeds[group.Title] = feeds;
            }
        }

        public FeedGroupModel[] GetSelectedFeeds()
        {
            var selectedGroups = new List<FeedGroupModel>(groups.Length);
            foreach (var group in groups)
            {
                var newFeeds = new List<FeedModel>(group.Feeds.Count);
                foreach (var feedModel in group.Feeds)
                {
                    var viewModels = (ObservableCollection<SearchResultViewModel>)Feeds[group.Title];
                    var viewModel = viewModels.First(vm => vm.Model.Url == feedModel.Url);
                    if (!viewModel.Exists && viewModel.IsSelected == true)
                    {
                        newFeeds.Add(feedModel);
                    }
                }
                if (newFeeds.Any())
                {
                    var newGroup = new FeedGroupModel
                    {
                        Title = group.Title,
                        Description = group.Description,
                        Feeds = newFeeds
                    };
                    selectedGroups.Add(newGroup);
                }
            }

            return selectedGroups.ToArray();
        }

        private async Task<string> readPopularFeeds()
        {
            string fileContent = null;
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Data/popularFeeds.json"));
            using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync()))
            {
                fileContent = await sRead.ReadToEndAsync();
            }

            return fileContent;
        }        

        private SearchResultViewModel createViewModel(FeedModel model)
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

            return viewModel;
        }        

        private bool feedExists(string url)
        {
            var repo = InstanceFactory.GetInstance<IFeedRepository>();
            bool exists = false;
            foreach (var group in repo.FeedGroups)
            {
                var feeds = group.Feeds.Where(feed => feed.Url != null);
                exists = feeds.Any(feed => feed.Url.Equals(url, StringComparison.Ordinal));
                if (exists)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
