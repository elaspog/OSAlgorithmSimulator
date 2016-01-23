using Simulator.Infrastructure.Views;
using System.Windows.Controls;

namespace MemoryAllocator.Views
{
    /// <summary>
    /// Interaction logic for MA_SimulationView.xaml
    /// </summary>
    public partial class MA_SimulationView : UserControl, IModuleViewBase
    {
        public MA_SimulationView()
        {
            InitializeComponent();
        }


        public string GetViewsDataContextAsPropertyNameOfModuleViewModel()
        {
            return "SimulatorViewModel";
        }
    }
}
