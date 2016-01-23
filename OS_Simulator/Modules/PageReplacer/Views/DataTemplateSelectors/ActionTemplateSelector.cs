using PageReplacer.Models;
using System.Windows;
using System.Windows.Controls;

namespace PageReplacer.Views.DataTemplateSelectors
{
    public class ActionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AccessPageTemplate { get; set; }
        public DataTemplate SetRbitOnPageTemplate { get; set; }
        public DataTemplate SetMbitOnPageTemplate { get; set; }
        public DataTemplate RemoveRbitOnPageTemplate { get; set; }
        public DataTemplate RemoveMbitOnPageTemplate { get; set; }
        public DataTemplate PeriodRemoveAllRbitTemplate { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (((PageActionBase)item).GetType() == typeof(PageAccess))
            {
                return AccessPageTemplate;
            }
            if (((PageActionBase)item).GetType() == typeof(PageSetRbit))
            {
                return SetRbitOnPageTemplate;
            }
            if (((PageActionBase)item).GetType() == typeof(PageSetMbit))
            {
                return SetMbitOnPageTemplate;
            }
            if (((PageActionBase)item).GetType() == typeof(PageRemoveRbit))
            {
                return RemoveRbitOnPageTemplate;
            }
            if (((PageActionBase)item).GetType() == typeof(PageRemoveMbit))
            {
                return RemoveMbitOnPageTemplate;
            }
            if (((PageActionBase)item).GetType() == typeof(PeriodRemoveAllRbit))
            {
                return PeriodRemoveAllRbitTemplate;
            }


            
            return null;
        }
    }
}
