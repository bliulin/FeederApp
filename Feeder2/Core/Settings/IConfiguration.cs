using Feeder.Common.Settings.Model;

namespace Feeder.Common.Settings
{
    public interface IConfiguration
    {
        bool TelemetryEnabled
        {
            get; set;
        }

        bool DisplayImages
        {
            get; set;
        }

        bool AutoUpdate { get; set; }

        TextAlignment TextAlignment { get; set; }

        bool KeepOnlyLatestArticles { get; set; }
        bool ArticleQuickView { get; set; }
    }
}
