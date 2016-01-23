using System.Windows;
using System.Windows.Controls;
using VirtualAddressMapper.Models;

namespace VirtualAddressMapper.Views
{
    public class ActionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AddPageActionTemplate { get; set; }
        public DataTemplate RemovePageActionTemplate { get; set; }
        public DataTemplate AddAddressActionTemplate { get; set; }
        public DataTemplate RemoveAddressActionTemplate { get; set; }
        public DataTemplate ResolveAddressActionTemplate { get; set; }



        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item.GetType() == typeof(AddPageToMemory))
            {
                return AddPageActionTemplate;
            }
            if (item.GetType() == typeof(RemovePageFromMemory))
            {
                return RemovePageActionTemplate;
            }
            if (item.GetType() == typeof(AddPageToAssociativeMemory))
            {
                return AddAddressActionTemplate;
            }
            if (item.GetType() == typeof(RemovePageFromAssociativeMemory))
            {
                return RemoveAddressActionTemplate;
            }
            if (item.GetType() == typeof(ResolveVirtualAddress))
            {
                return ResolveAddressActionTemplate;
            }
            return null;
        }

    }
}
