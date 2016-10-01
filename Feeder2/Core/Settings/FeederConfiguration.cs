using Feeder.Common.Settings.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Feeder.Common.Settings
{
    public class FeederConfiguration : IConfiguration
    {
        private ApplicationDataContainer settings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public bool ArticleQuickView
        {
            get
            {
                if (!settings.Values.ContainsKey(Constants.ARTICLE_QUICK_VIEW))
                {
                    settings.Values[Constants.ARTICLE_QUICK_VIEW] = true;
                }

                return (bool)settings.Values[Constants.ARTICLE_QUICK_VIEW];
            }
            set
            {
                settings.Values[Constants.ARTICLE_QUICK_VIEW] = value;
            }
        }

        public bool AutoUpdate
        {
            get
            {
                if (!settings.Values.ContainsKey(Constants.AUTO_UPDATE))
                {
                    settings.Values[Constants.AUTO_UPDATE] = true;
                }

                return (bool)settings.Values[Constants.AUTO_UPDATE];
            }
            set
            {
                settings.Values[Constants.AUTO_UPDATE] = value;
            }
        }

        public bool DisplayImages
        {
            get
            {
                if (!settings.Values.ContainsKey(Constants.DISPLAY_IMAGES))
                {
                    settings.Values[Constants.DISPLAY_IMAGES] = true;
                }
                return (bool)settings.Values[Constants.DISPLAY_IMAGES];
            }
            set
            {
                settings.Values[Constants.DISPLAY_IMAGES] = value;
            }
        }

        public bool KeepOnlyLatestArticles
        {
            get
            {
                if (!settings.Values.ContainsKey(Constants.KEEP_ONLY_LATEST_ARTICLES))
                {
                    settings.Values[Constants.KEEP_ONLY_LATEST_ARTICLES] = true;
                }
                return (bool)settings.Values[Constants.KEEP_ONLY_LATEST_ARTICLES];
            }
            set
            {
                settings.Values[Constants.KEEP_ONLY_LATEST_ARTICLES] = value;
            }
        }

        public bool TelemetryEnabled
        {
            get
            {
                if (!settings.Values.ContainsKey(Constants.TELEMETRY_ENABLED))
                {
                    settings.Values[Constants.TELEMETRY_ENABLED] = true;
                }
                return (bool)settings.Values[Constants.TELEMETRY_ENABLED];
            }
            set
            {
                settings.Values[Constants.TELEMETRY_ENABLED] = value;
            }
        }

        public TextAlignment TextAlignment
        {
            get
            {
                if (!settings.Values.ContainsKey(Constants.TEXT_ALIGN))
                {
                    settings.Values[Constants.TEXT_ALIGN] = (int)TextAlignment.Left;
                }
                return (TextAlignment)settings.Values[Constants.TEXT_ALIGN];
            }
            set
            {
                settings.Values[Constants.TEXT_ALIGN] = (int)value;
            }
        }
    }
}
