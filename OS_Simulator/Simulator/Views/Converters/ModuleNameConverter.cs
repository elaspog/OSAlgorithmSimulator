using System;
using System.Globalization;
using System.Windows.Data;

namespace Simulator.Views.Converters
{
    public class ModuleNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = value.ToString();
            string [] stringParts = str.Split('.');

            foreach (string part in stringParts)
            {
                if (! part.Equals(stringParts[0]))
                {
                    return str;
                }
            }
            return stringParts[0];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
