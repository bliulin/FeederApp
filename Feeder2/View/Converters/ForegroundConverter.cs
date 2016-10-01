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
    public class ForegroundConverter : IValueConverter
    {
        private Brush colorUnselected = new SolidColorBrush(Colors.CadetBlue);
        private Brush colorSelected = new SolidColorBrush(Colors.GreenYellow);

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isSelected = value as bool?;
            if (isSelected == null)
            {
                return colorUnselected;
            }

            return isSelected.HasValue && isSelected.Value ? colorSelected : colorUnselected;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
