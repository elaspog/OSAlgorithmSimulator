using Simulator.Infrastructure.Views;
using System.Windows.Controls;

namespace VirtualAddressMapper.Views
{
    /// <summary>
    /// Interaction logic for VAM_StatisticsView.xaml
    /// </summary>
    public partial class VAM_StatisticsView : UserControl, IModuleViewBase
    {
        public VAM_StatisticsView()
        {
            InitializeComponent();
        }

        public string GetViewsDataContextAsPropertyNameOfModuleViewModel()
        {
            return "SimulatorViewModel";
        }
    }
}
