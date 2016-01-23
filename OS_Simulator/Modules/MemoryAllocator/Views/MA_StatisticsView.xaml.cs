using Simulator.Infrastructure.Views;
using System.Windows.Controls;

namespace MemoryAllocator.Views
{
    /// <summary>
    /// Interaction logic for MA_StatisticsView.xaml
    /// </summary>
    public partial class MA_StatisticsView : UserControl, IModuleViewBase
    {
        public MA_StatisticsView()
        {
            InitializeComponent();
        }


        public string GetViewsDataContextAsPropertyNameOfModuleViewModel()
        {
            return "SimulatorViewModel";
        }
    }
}
