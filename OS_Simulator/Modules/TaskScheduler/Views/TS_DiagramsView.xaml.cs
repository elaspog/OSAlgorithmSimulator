using Simulator.Infrastructure.Views;
using System.Windows.Controls;

namespace TaskScheduler.Views
{
    /// <summary>
    /// Interaction logic for DiagramsView.xaml
    /// </summary>
    public partial class TS_DiagramsView : UserControl, IModuleViewBase
    {
        public TS_DiagramsView()
        {
            InitializeComponent();
        }

        public string GetViewsDataContextAsPropertyNameOfModuleViewModel()
        {
            return "SimulatorViewModel";
        }
    }
}
