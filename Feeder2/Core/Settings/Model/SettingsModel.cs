using Feeder.Common.Factory;
using Feeder.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Feeder.Common.Settings.Model
{
    public class SettingsModel
    {
        public SettingsModel()
        {            
        }

        public TextAlignment[] TextAlignmentValues
        {
            get
            {
                return new[] { TextAlignment.Left, TextAlignment.Center, TextAlignment.Justify };
            }
        }

        public bool TelemetryEnabled { get; set; }

        public bool DisplayImages { get; set; }

        public bool AutoUpdate { get; set; }

        public TextAlignment SelectedTextAlignment { get; set; }

        public bool? KeepOnlyLatestArticles { get; set; }

        public bool ArticleQuickView { get; set; }

        public From[] KeepFeedsFrom
        {
            get
            {
                return new[] { From.LastDay, From.LastWeek, From.LastMonth };
            }
        }

        public From SelectedFrom { get; set; }

        public void SaveSettings()
        {
            var config = InstanceFactory.GetInstance<IConfiguration>();

            config.DisplayImages = DisplayImages;
            config.TextAlignment = SelectedTextAlignment;
            config.TelemetryEnabled = TelemetryEnabled;
            config.AutoUpdate = AutoUpdate;
            config.KeepOnlyLatestArticles = KeepOnlyLatestArticles.Value;
            config.ArticleQuickView = ArticleQuickView;
        }

        public static SettingsModel ReadSettings()
        {
            var config = InstanceFactory.GetInstance<IConfiguration>();

            var settings = new SettingsModel
            {
                AutoUpdate = config.AutoUpdate,
                DisplayImages = config.DisplayImages,
                TelemetryEnabled = config.TelemetryEnabled,
                SelectedTextAlignment = config.TextAlignment,
                KeepOnlyLatestArticles = config.KeepOnlyLatestArticles,
                ArticleQuickView = config.ArticleQuickView
            };

            return settings;
        }
    }
}
