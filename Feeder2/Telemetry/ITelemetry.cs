using Feeder.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feeder.PivotApp.Telemetry
{
    interface ITelemetry
    {
        void TrackException(Exception ex);
        void TrackEvent(string eventName, Dictionary<string, string> properties = null,
            Dictionary<string, double> metrics = null);
        void TrackSearchEvent(string searchTerm);
        void TrackFeedAddedFromSearchEvent(FeedModel feed);
        void TrackFeedAddedManuallyEvent(FeedModel feed);
        void TrackFeedAddedFromList(FeedModel feed);
        void TrackFeedDeleted(FeedModel feed);
        void TrackFeedUpdated(FeedModel feed);
    }
}
