using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace TaskScheduler.Views
{
    public class NormalListOrEmptyListSelector : DataTemplateSelector
    {

        public DataTemplate NormalListTemplate { get; set; }
        public DataTemplate EmptylListTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {

            ObservableCollection<string> tmp = (ObservableCollection<string>)item;

            if (tmp.Count == 0)
            {
                return EmptylListTemplate;
            }

            return NormalListTemplate;
        }
    }
}
