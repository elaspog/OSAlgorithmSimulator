using MemoryAllocator.Models;
using System.Windows;
using System.Windows.Controls;

namespace MemoryAllocator.Views
{
    public class PartitionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate UsedPartitionTemplate { get; set; }
        public DataTemplate FreePartitionTemplate { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (((PartitionRecord)item).PartitionType == PartitionType.Used)
            {
                return UsedPartitionTemplate;
            }
            if (((PartitionRecord)item).PartitionType == PartitionType.Free)
            {
                return FreePartitionTemplate;
            }
            return null;
        }
    }
}
