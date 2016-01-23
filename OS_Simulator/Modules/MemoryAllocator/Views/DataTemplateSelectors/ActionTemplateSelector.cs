using MemoryAllocator.Models;
using System.Windows;
using System.Windows.Controls;

namespace MemoryAllocator.Views
{
    public class ActionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FreeActionTemplate { get; set; }
        public DataTemplate AllocateActionTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item.GetType() == typeof(FreeAction))
            {
                return FreeActionTemplate;
            }
            if (item.GetType() == typeof(AllocateAction))
            {
                return AllocateActionTemplate;
            }
            return null;
        }
       
    }
}
