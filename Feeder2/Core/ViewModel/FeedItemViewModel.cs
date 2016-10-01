using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Feeder.DataModel;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Feeder.Common.Factory;
using Feeder.Common.Utils;
using Feeder.Common.Settings;
using Feeder.Common.Repository;

namespace Feeder.Common.ViewModel
{
    public class FeedItemViewModel : BaseViewModel
    {
        private ImageSource mHeadlineImage;
        private GridLength mImageSize;

        public FeedItemModel Model { get; set; }

        public string Title
        {
            get; set;
        }

        public string Subtitle
        {
            get; set;
        }

        public string ShortDescription
        {
            get
            {
                return Model.ShortDescription;
            }
        }

        public int ShortDescriptionHeight
        {
            get; set;
        }

        public bool IsRead
        {
            get
            {
                return Model.IsRead;
            }
            set
            {
                Model.IsRead = value;
                OnNotifyPropertyChanged("IsRead");
            }
        }

        private bool mIsSaved;
        public bool IsSaved
        {
            get
            {
                return mIsSaved;
            }
            set
            {
                mIsSaved = value;
                OnNotifyPropertyChanged("IsSaved");
            }
        }


        public ImageSource HeadlineImage
        {
            get
            {
                return mHeadlineImage;
            }
            set
            {
                mHeadlineImage = value;
                OnNotifyPropertyChanged("HeadlineImage");

                ImageColumnSize = mHeadlineImage != null ? new GridLength(64) : new GridLength(0);
            }
        }

        public GridLength ImageColumnSize
        {
            get
            {
                return mImageSize;
            }
            set
            {
                mImageSize = value;
                OnNotifyPropertyChanged("ImageColumnSize");
            }
        }

        public string Content { get; set; }

        public string ItemUri
        {
            get
            {
                return Model.ItemUri;
            }
        }

        public static FeedItemViewModel Create(FeedItemModel model)
        {
            var viewModel = new FeedItemViewModel();

            viewModel.Model = model;

            viewModel.Title = model.Title;
            string elapsedTime = DateTimeUtils.GetElapsedTimeDescription(model.PublishDate).Replace("Last updated ", "");

            viewModel.Subtitle = string.Format("{0} / {1}", elapsedTime, model.Author);

            if (!string.IsNullOrWhiteSpace(model.ImageUri))
            {
                var image = new BitmapImage(new Uri(model.ImageUri));
                viewModel.HeadlineImage = image as ImageSource;
            }

            viewModel.Content = HtmlStyling.StyleHtmlContent(model.Summary);

            var config = InstanceFactory.GetInstance<IConfiguration>();
            viewModel.ShortDescriptionHeight = config.ArticleQuickView && 
                !string.IsNullOrEmpty(viewModel.ShortDescription) ? 85 : 0;

            var repo = InstanceFactory.GetInstance<IFeedRepository>();
            viewModel.IsSaved = repo.SavedArticles.Exists(a => a.ItemUri == viewModel.Model.ItemUri);

            return viewModel;
        }
    }
}
