using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feeder.Common.Utils
{
    public static class DateTimeUtils
    {
        public static string GetElapsedTimeDescription(DateTime? publishedDate)
        {
            if (publishedDate == null)
            {
                return "Not updated";
            }

            string desc = "Last updated";

            DateTime now = DateTime.UtcNow;
            var diff = now.Subtract(publishedDate.Value);

            int minutes = (int)Math.Round(diff.TotalMinutes);
            if (minutes < 1)
            {
                return desc + " just now";
            }
            if (minutes < 60)
            {
                string minutesDesc = minutes == 1 ? "minute" : "minutes";
                return string.Format("{0} {1} {2} ago", desc, minutes, minutesDesc);
            }

            int hours = (int)Math.Round(diff.TotalHours);
            if (hours < 24)
            {
                string hoursDesc = hours == 1 ? "hour" : "hours";
                return string.Format("{0} {1} {2} ago", desc, hours, hoursDesc);
            }

            int days = (int)Math.Round(diff.TotalDays);
            string daysDesc = days == 1 ? "day" : "days";
            return string.Format("{0} {1} {2} ago", desc, days, daysDesc);
        }
    }
}
