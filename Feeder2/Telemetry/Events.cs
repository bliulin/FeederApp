using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feeder.PivotApp.Telemetry
{
    static class Events
    {
        public const string SUSPENDING = "SUSPENDING";
        public const string SEARCH = "SEARCH";
        public const string FEED_ADDED_SEARCH = "FEED_ADDED_SEARCH";
        public const string FEED_ADDED_MANUAL = "FEED_ADDED_MANUAL";
        public const string FEED_ADDED_LIST = "FEED_ADDED_LIST";
        public const string OPEN_ARTICLE = "OPEN_ARTICLE";
        public const string SHARE_ARTICLE = "SHARE_ARTICLE";
        public const string SAVE_ARTICLE = "SAVE_ARTICLE";
        public const string DELETE_FEED = "DELETE_FEED";
        public const string FEED_UPDATED = "FEED_UPDATE";
    }
}
