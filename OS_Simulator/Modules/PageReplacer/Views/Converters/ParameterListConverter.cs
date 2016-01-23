using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace PageReplacer.Views.Converters
{
    public class ParameterListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<string> parameterList = (ObservableCollection<string>) value;

            string str = "";

            bool first = true;
            foreach (string param in parameterList)
            {
                if (!first)
                {
                    str += ", ";
                }
                str += param;
                first = false;
            }

            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
