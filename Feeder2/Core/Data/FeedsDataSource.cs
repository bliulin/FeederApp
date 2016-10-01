using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Syndication;
using Feeder.Common.Factory;
using Feeder.DataModel;
using Feeder.Common.Utils;
using System.Threading;
using System.Net;
using System.Net.Http;
using System.IO;
using Windows.Storage;
using System.Text.RegularExpressions;

namespace Feeder.Common.Data
{
    public class FeedsDataSource : IFeedsDataSource
    {
        private SyndicationClient syndClient = new SyndicationClient();

        public async Task<FeedModel> GetFeedItemsAsync(string url)
        {
            var httpClient = new System.Net.Http.HttpClient();
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            var sf = parseContentIntoFeed(content);

            return ModelFactory.Create(sf, null);
        }

        private SyndicationFeed parseContentIntoFeed(string content)
        {
            SyndicationFeed sf = new SyndicationFeed();

            try
            {
                sf.Load(content);
            }
            catch (Exception)
            {
                var match = Regex.Match(content, @"<rss(.|\s)+\/rss>");
                if (match.Success)
                {
                    string minified = match.Value.Replace("\n", "");
                    sf.Load(minified);
                }
                else throw;
            }

            return sf;
        }        

        public async Task<FeedModel> GetFeedItemsAsync(string url, CancellationToken token)
        {
            var httpClient = new System.Net.Http.HttpClient();
            var response = await httpClient.GetAsync(url, token);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            var sf = parseContentIntoFeed(content);

            return ModelFactory.Create(sf, null);
        }        
    }
}
