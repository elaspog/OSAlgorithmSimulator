using PageReplacer.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PageReplacer.Views.DataTemplateSelectors
{
    public class GridViewTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BasicTemplate { get; set; }
        public DataTemplate ReducedTemplateWithTime { get; set; }
        public DataTemplate ReducedTemplateWithLFUCounter { get; set; }
        

        private Type simulationType;
        public Type SimulationType
        {
            get { return simulationType; }
            set { simulationType = value; }
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            SimulationType = ((Type)((FrameworkElement)container).FindResource("SimulationType"));

            if (SimulationType == typeof(PageReplacerFifo) )
            {
                return BasicTemplate;
            }
            if (SimulationType == typeof(PageReplacerLastFrequentlyUsed) )
            {
                return ReducedTemplateWithLFUCounter;
            }
            if (SimulationType == typeof(PageReplacerLastRecentlyUsed) )
            {
                return ReducedTemplateWithTime;
            }
            if (SimulationType == typeof(PageReplacerNotRecentlyUsed) )
            {
                return BasicTemplate;
            }
            if (SimulationType == typeof(PageReplacerOptimal) )
            {
                return BasicTemplate;
            }
            if (SimulationType == typeof(PageReplacerSecondChance))
            {
                return BasicTemplate;
            }
            return null;

        }
    }
}
