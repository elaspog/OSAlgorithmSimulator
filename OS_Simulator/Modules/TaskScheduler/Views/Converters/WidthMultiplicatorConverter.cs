using System;
using System.Windows.Data;

namespace TaskScheduler.Views
{
    public class WidthMultiplicatorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int tmp = (int)value;

            return tmp * 20;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
