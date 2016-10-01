using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Syndication;
using Feeder.DataModel;
using Microsoft.Practices.ObjectBuilder2;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using Windows.Data.Xml.Dom;
using Feeder.Common.Utils;

namespace Feeder.Common.Factory
{
    public class ModelFactory
    {
        private const string URL_REGEX = @"\b(?:https?://|www\.)\S+\b";

        #region Statics

        public static FeedItemModel Create(SyndicationItem syndicationItem)
        {
            var model = new FeedItemModel();

            model.Id = syndicationItem.Id;
            model.PublishDate = syndicationItem.PublishedDate.UtcDateTime;
            model.Title = syndicationItem.Title != null ? syndicationItem.Title.Text : "";
            model.Summary = syndicationItem.Summary != null ? syndicationItem.Summary.Text : "";
            model.ImageUri = getImageUrlFromFeedItem(syndicationItem);
            model.ItemUri = getItemUri(syndicationItem);
            model.Author = getAuthor(syndicationItem);

            model.ShortDescription = syndicationItem.GetSummary();

            return model;
        }

        public static FeedModel Create(SyndicationFeed syndicationFeed, string feedSourceId)
        {
            var model = new FeedModel();

            model.Id = feedSourceId;
            model.Title = syndicationFeed.Title != null ? syndicationFeed.Title.Text : "";

            model.Description = syndicationFeed.Subtitle != null
                    ? syndicationFeed.Subtitle.Text
                    : "";

            model.ImageUri = syndicationFeed.ImageUri != null
                    ? syndicationFeed.ImageUri.ToString()
                    : "";

            model.Items = new List<FeedItemModel>(syndicationFeed.Items.Count());
            syndicationFeed.Items.ForEach(item => model.Items.Add(Create(item)));

            return model;
        }

        private static string getItemUri(SyndicationItem syndicationItem)
        {
            string itemUrl = string.Empty;

            if (syndicationItem.ItemUri != null)
            {
                itemUrl = syndicationItem.ItemUri.AbsoluteUri;
            }
            else
            {
                if (syndicationItem.CommentsUri != null)
                {
                    itemUrl = syndicationItem.CommentsUri.AbsoluteUri;
                }
                else if (syndicationItem.Id != null)
                {
                    itemUrl = syndicationItem.Id;
                }
            }

            return extractUrl(itemUrl);
        }

        private static string extractUrl(string text)
        {
            var parser = new Regex(URL_REGEX, RegexOptions.IgnoreCase);
            var match = parser.Match(text);
            if (match.Success)
            {
                return match.Value;
            }

            return null;
        }

        private static string getAuthor(SyndicationItem syndicationItem)
        {
            if (syndicationItem.Authors.Count > 0)
            {
                return string.IsNullOrWhiteSpace(syndicationItem.Authors[0].Name) ? syndicationItem.Authors[0].Email : "";
            }
            return string.Empty;
        }

        private static string getImageUrlFromFeedItem(SyndicationItem syndicationItem)
        {
            string imageUrl = string.Empty;

            try
            {
                SyndicationLink imageLink = null;
                foreach (var link in syndicationItem.Links)
                {
                    if (!string.IsNullOrWhiteSpace(link.MediaType) && link.MediaType.ToLower() == "image/jpeg")
                    {
                        imageLink = link;
                    }
                }

                if (imageLink != null)
                {
                    imageUrl = imageLink.Uri.ToString();
                }
                else
                {
                    // try to determine the image from the feed item's summary
                    var description = syndicationItem.Summary != null ? syndicationItem.Summary.Text.Trim() : "";
                    imageUrl = getImageUrlFromText(description);
                }
            }
            catch (Exception)
            {                
            }

            return imageUrl;
        }        

        private static string getImageUrlFromText(string text)
        {
            string imageUrl = string.Empty;

            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            string pattern = @"<img\s+[^>]*src=""([^""]*)""[^>]*>";
            var matches = Regex.Matches(text, pattern);
            if (matches.Count > 0)
            {
                if (matches[0].Groups.Count > 1)
                {
                    string url = matches[0].Groups[1].Value;
                    return extractUrl(url);
                }

                return matches[0].Groups[1].Value;
            }

            return null;            
        }

        #endregion
    }
}
