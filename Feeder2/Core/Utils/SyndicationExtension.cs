using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Syndication;

namespace Feeder.Common.Utils
{
    public static class SyndicationItemExtension
    {
        public static string GetSummary(this SyndicationItem feedArticle)
        {            
            var xmlDoc = feedArticle.GetXmlDocument(SyndicationFormat.Rss20);
            var summary = xmlDoc.GetElementsByTagName("summary").FirstOrDefault();
            if (summary != null)
            {
                return WebUtility.HtmlDecode(summary.InnerText);                
            }                     

            return null;
        }
    }
}
