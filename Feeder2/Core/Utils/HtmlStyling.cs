using Feeder.Common.Factory;
using Feeder.Common.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feeder.Common.Utils
{
    public static class HtmlStyling
    {
        public static string StyleHtmlContent(string html)
        {
            var config = InstanceFactory.GetInstance<IConfiguration>();
            string alignment = config.TextAlignment.ToString().ToLower();
            html = $"<span style=\"color: white; font-family: 'Segoe UI'; text-align: {alignment};\">{html}</span>";
            return html;
        }
    }
}
