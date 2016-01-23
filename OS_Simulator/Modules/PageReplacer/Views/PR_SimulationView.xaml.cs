using Simulator.Infrastructure.Views;
using System.Windows.Controls;

namespace PageReplacer.Views
{
    /// <summary>
    /// Interaction logic for PR_SimulationView.xaml
    /// </summary>
    public partial class PR_SimulationView : UserControl, IModuleViewBase
    {
        public PR_SimulationView()
        {
            InitializeComponent();
        }

        public string GetViewsDataContextAsPropertyNameOfModuleViewModel()
        {
            return "SimulatorViewModel"; 
        }
    }
}
