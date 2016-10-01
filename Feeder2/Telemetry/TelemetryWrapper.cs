using Feeder.Common.Settings;
using Feeder.DataModel;
using Feeder.PivotApp.Settings;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feeder.PivotApp.Telemetry
{
    class TelemetryWrapper : ITelemetry
    {
        private TelemetryClient mClient;
        private bool mEnabled;

        public TelemetryWrapper(IConfiguration config)
        {
            mClient = new TelemetryClient();
            mClient.InstrumentationKey = "a251d0b5-142d-4c4b-b3f3-3d41114b8654";
            mEnabled = config.TelemetryEnabled;
        }

        public void TrackException(Exception ex)
        {
            if (!mEnabled)
            {
                return;
            }
            mClient.TrackException(ex);            
        }

        public void TrackEvent(string eventName, Dictionary<string, string> properties = null, 
            Dictionary<string, double> metrics = null)
        {
            if (!mEnabled)
            {
                return;
            }
            mClient.TrackEvent(eventName, properties, metrics);
        }

        public void TrackSearchEvent(string searchTerm)
        {
            if (!mEnabled)
            {
                return;
            }

            var parameters = new Dictionary<string, string>();
            parameters.Add("searchKey", searchTerm);
            mClient.TrackEvent(Events.SEARCH, parameters);
        }

        public void TrackFeedAddedFromSearchEvent(FeedModel feed)
        {
            trackFeedEvent(Events.FEED_ADDED_SEARCH, feed);
        }

        public void TrackFeedAddedManuallyEvent(FeedModel feed)
        {
            trackFeedEvent(Events.FEED_ADDED_MANUAL, feed);
        }

        public void TrackFeedAddedFromList(FeedModel feed)
        {
            trackFeedEvent(Events.FEED_ADDED_LIST, feed);
        }

        public void TrackFeedUpdated(FeedModel feed)
        {
            trackFeedEvent(Events.FEED_UPDATED, feed);
        }

        public void TrackFeedDeleted(FeedModel feed)
        {
            trackFeedEvent(Events.DELETE_FEED, feed);
        }

        private void trackFeedEvent(string eventName, FeedModel feed)
        {
            if (!mEnabled)
            {
                return;
            }

            string feedContent = JsonConvert.SerializeObject(feed);
            var parameters = new Dictionary<string, string>();
            parameters.Add("feed", feedContent);
            mClient.TrackEvent(eventName, parameters);
        }
    }
}
