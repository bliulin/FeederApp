using System;
using Windows.UI.Xaml.Data;

namespace Feeder.PivotApp.View.Converters
{
    class DebugConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
