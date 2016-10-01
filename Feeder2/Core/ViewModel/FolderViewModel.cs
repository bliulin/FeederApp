using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feeder.Common.Model;
using Feeder.Common.Factory;
using Feeder.Common.Repository;

namespace Feeder.Common.ViewModel
{
    public class FolderViewModel : BaseListItem
    {
        string name;

        public new string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                Model.Title = value;
                OnNotifyPropertyChanged("Name");
            }
        }

        public override Task LoadData()
        {
            return null;
        }

        public FeedGroupModel Model
        {
            get;
            set;
        }

        public static FolderViewModel Create(FeedGroupModel model)
        {
            return new FolderViewModel
            {
                Model = model,
                Name = model.Title,
                Summary = model.Description,
                ItemsCount = (from feed in model.Feeds
                              from article in feed.Items
                              where !article.IsRead
                              select article).Count(),
                SettingsCommand = new RelayCommand(() => { }, () => false)
            };
        }
    }
}
