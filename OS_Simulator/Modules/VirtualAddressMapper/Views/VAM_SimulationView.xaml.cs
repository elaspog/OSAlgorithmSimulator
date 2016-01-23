using Simulator.Infrastructure.Views;
using System.Windows.Controls;

namespace VirtualAddressMapper.Views
{
    /// <summary>
    /// Interaction logic for VAM_SimulationView.xaml
    /// </summary>
    public partial class VAM_SimulationView : UserControl, IModuleViewBase
    {
        public VAM_SimulationView()
        {
            InitializeComponent();
        }

        public string GetViewsDataContextAsPropertyNameOfModuleViewModel()
        {
            return "SimulatorViewModel";
        }
        
    }
}
