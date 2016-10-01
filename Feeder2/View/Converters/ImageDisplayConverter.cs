using Feeder.Common.Factory;
using Feeder.Common.Settings;
using Feeder.PivotApp.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Feeder.PivotApp.View.Converters
{
    class ImageDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var config = InstanceFactory.GetInstance<IConfiguration>();
            if (!config.DisplayImages)
            {
                return new GridLength(0);
            }

            return (GridLength)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
