using System;
using System.Windows.Data;

namespace VirtualAddressMapper.Views.Converters
{
    class InteralToStringStartingWithZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var count = (int)value;
            string str = "[0-" + (count-1).ToString() + "]";
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
