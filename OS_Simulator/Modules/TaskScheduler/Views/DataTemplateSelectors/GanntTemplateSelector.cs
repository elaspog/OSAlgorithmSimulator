using System.Windows;
using System.Windows.Controls;
using TaskScheduler.Models;

namespace TaskScheduler.Views
{
    public class GanntTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FirstGanntElementTemplate { get; set; }
        public DataTemplate OtherGanntElementTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {

            ProcessWithQueueAssigns tmp = (ProcessWithQueueAssigns)item;

            if (tmp.ProcessName == "")
            {
                return FirstGanntElementTemplate;
            }
            return OtherGanntElementTemplate;
        }
    }
}
