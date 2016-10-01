using Feeder.Common.Factory;
using Feeder.Common.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feeder.DataModel;
using System.ComponentModel;

namespace Feeder.Common.ViewModel
{
    public class FeedEditViewModel : INotifyPropertyChanged
    {
        public string FeedName { get; set; }

        public string UrlAddress { get; set; }

        public ObservableCollection<string> Folders { get; set; }

        public string NewFolderName
        {
            get;
            set;
        }

        public IFeedRepository FeedRepository
        {
            get
            {
                return InstanceFactory.GetInstance<IFeedRepository>();
            }
        }

        public string SelectedFolderName
        {
            get;
            set;
        }

        public FeedEditViewModel()
        {
            Folders = new ObservableCollection<string>();
        }

        public void LoadData()
        {
            var folderNames = FeedRepository.GetGroupNames();
            foreach (var folderName in folderNames)
            {
                Folders.Add(folderName);
            }

            Folders.Insert(0, "new folder...");
        }

        public async Task SaveData(FeedModel editFeedModel = null)
        {
            string folderName = SelectedFolderName == "new folder..."
                    ? NewFolderName
                    : SelectedFolderName;

            var feedModel = editFeedModel;
            if (feedModel == null)
            {
                feedModel = new FeedModel();
            }

            feedModel.Name = FeedName;
            feedModel.Title = FeedName;
            feedModel.Description = FeedName;
            feedModel.Url = UrlAddress;
            feedModel.ParentGroupName = folderName;

            await FeedRepository.SaveFeed(feedModel);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
