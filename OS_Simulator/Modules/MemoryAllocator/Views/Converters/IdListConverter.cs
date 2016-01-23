using System;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace MemoryAllocator.Views.Converters
{
    public class IdListConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var list = (ObservableCollection<int>)value;
            string str = "ID: (";

            bool firstElement = true;

            foreach (var item in list)
            {
                if (firstElement)
                {
                    str += item;
                }
                else
                {
                    str += ", " + item;
                }
                firstElement = false;
            }
            str += ")";
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
