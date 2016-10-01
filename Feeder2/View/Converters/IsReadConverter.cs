using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Feeder.PivotApp.View.Converters
{
    class IsReadConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var colorUnRead = new SolidColorBrush(Color.FromArgb(255, 0, 165, 255));
            var colorRead = new SolidColorBrush(Color.FromArgb(255, 64, 64, 64));

            var hasRead = (bool)value;
            return hasRead ? colorRead : colorUnRead;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
