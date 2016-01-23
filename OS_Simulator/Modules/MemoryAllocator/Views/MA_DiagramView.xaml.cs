using Simulator.Infrastructure.Views;
using System.Windows.Controls;


namespace MemoryAllocator.Views
{
    /// <summary>
    /// Interaction logic for MA_DiagramView.xaml
    /// </summary>
    public partial class MA_DiagramView : UserControl, IModuleViewBase
    {
        public MA_DiagramView()
        {
            InitializeComponent();
        }



        public string GetViewsDataContextAsPropertyNameOfModuleViewModel()
        {
            return null;
        }
    }
}
