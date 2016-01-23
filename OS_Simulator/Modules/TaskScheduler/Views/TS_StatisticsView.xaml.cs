using Simulator.Infrastructure.Views;
using System.Windows.Controls;

namespace TaskScheduler.Views
{
    /// <summary>
    /// Interaction logic for StatisticsView.xaml
    /// </summary>
    public partial class TS_StatisticsView : UserControl, IModuleViewBase
    {
        public TS_StatisticsView()
        {
            InitializeComponent();
        }

        public string GetViewsDataContextAsPropertyNameOfModuleViewModel()
        {
            return "SimulatorViewModel";
        }
    }
}
