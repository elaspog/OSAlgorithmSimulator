using Simulator.Infrastructure.Views;
using System.Windows.Controls;
namespace PageReplacer.Views
{
    /// <summary>
    /// Interaction logic for PR_StatisticsView.xaml
    /// </summary>
    public partial class PR_StatisticsView : UserControl, IModuleViewBase
    {
        public PR_StatisticsView()
        {
            InitializeComponent();
        }

        public string GetViewsDataContextAsPropertyNameOfModuleViewModel()
        {
            return "SimulatorViewModel";
        }
    }
}
