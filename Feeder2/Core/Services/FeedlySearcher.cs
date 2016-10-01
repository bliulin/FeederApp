using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feeder.DataModel;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace Feeder.Common.Services
{
    public class FeedlySearcher : IFeedSearcher
    {
        private const string BASE_URL= "http://cloud.feedly.com/v3/search/feeds";

        public async Task<FeedModel[]> SearchAsync(string query)
        {
            string uri = constructUrl(query);
            var response = await getStringFromUrl(uri);
            var list = getFeedModelList(response);
            return list;
        }

        private FeedModel[] getFeedModelList(string response)
        {
            var list = new List<FeedModel>();

            var jObject = JObject.Parse(response);
            var resultArray = (JArray)jObject["results"];
            foreach (var item in resultArray)
            {
                var model = new FeedModel();
                model.Title = model.Name = (string) item["title"];
                model.Description = (string) item["description"];
                model.Url = ((string)item["feedId"]).Replace("feed/", "");
                model.ImageUri = (string)item["iconUrl"];
                list.Add(model);
            }

            return list.ToArray();
        }

        private string constructUrl(string query)
        {
            return string.Concat(BASE_URL, "?", "query=" + query);
        }

        private async Task<string> getStringFromUrl(string uri)
        {
            var httpClient = new HttpClient();
            var result = await httpClient.GetAsync(new Uri(uri));
            result.EnsureSuccessStatusCode();

            string xml = await result.Content.ReadAsStringAsync();
            return xml;
        }
    }
}
