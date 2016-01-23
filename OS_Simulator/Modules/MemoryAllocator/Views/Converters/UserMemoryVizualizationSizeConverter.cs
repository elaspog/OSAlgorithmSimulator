using System;
using System.Windows.Data;

namespace MemoryAllocator.Views.Converters
{
    public class UserMemoryVizualizationSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Int32 sizeStr = (Int32)value;
            return (float)sizeStr * 2.5;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
