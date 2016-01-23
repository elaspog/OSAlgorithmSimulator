using MemoryAllocator.Models;
using System.Windows;
using System.Windows.Controls;

namespace MemoryAllocator.Views
{
    public class VizualizeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FreePartitionVizualizerTemplate { get; set; }
        public DataTemplate UsedPartitionVizualizerTemplate { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (((PartitionRecord)item).PartitionType == PartitionType.Used)
            {
                return UsedPartitionVizualizerTemplate;
            }
            if (((PartitionRecord)item).PartitionType == PartitionType.Free)
            {
                return FreePartitionVizualizerTemplate;
            }
            return null;
        }
    }
}
