using System.Windows;
using System.Windows.Controls;
using VirtualAddressMapper.Models;

namespace VirtualAddressMapper.Views
{
    public class ProcessBarItemSelector : DataTemplateSelector
    {
        public DataTemplate FreePageVizualizerTemplate { get; set; }
        public DataTemplate UsedPageVizualizerProcTemplate { get; set; }
        public DataTemplate FreeNumberedVizualizerTemplate { get; set; }
        public DataTemplate UsedPageVizualizerMemTemplate { get; set; }
        

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {

            IndicatorRecord tmp = (IndicatorRecord)item;

            if (tmp.Flag == true)
            {
                if (tmp.Frame != null)
                {
                    return UsedPageVizualizerMemTemplate;
                }
                return UsedPageVizualizerProcTemplate;
            }
            if (tmp.Flag == false && tmp.Page == null)
            {
                return FreePageVizualizerTemplate;
            }
            if (tmp.Flag == false && tmp.Page != null)
            {
                return FreeNumberedVizualizerTemplate;
            }

            return null;
        }
    }
}
