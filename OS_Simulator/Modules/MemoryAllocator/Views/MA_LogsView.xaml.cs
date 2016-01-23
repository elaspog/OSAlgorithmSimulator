using Simulator.Infrastructure.Views;
using System.Windows.Controls;

namespace MemoryAllocator.Views
{
    /// <summary>
    /// Interaction logic for MA_LogsView.xaml
    /// </summary>
    public partial class MA_LogsView : UserControl, IModuleViewBase
    {
        public MA_LogsView()
        {
            InitializeComponent();
        }

        public string GetViewsDataContextAsPropertyNameOfModuleViewModel()
        {
            return null;
        }
    }
}
