using Simulator.Infrastructure.Views;
using System.Windows.Controls;

namespace TaskScheduler.Views
{
    /// <summary>
    /// Interaction logic for SimulationView.xaml
    /// </summary>
    public partial class TS_SimulationView : UserControl, IModuleViewBase
    {
        public TS_SimulationView()
        {
            InitializeComponent();
        }

        public string GetViewsDataContextAsPropertyNameOfModuleViewModel()
        {
            return "SimulatorViewModel";
        }
    }
}
