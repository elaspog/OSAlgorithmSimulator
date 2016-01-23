using Simulator.Infrastructure.Views;
using System.Windows.Controls;

namespace PageReplacer.Views
{
    /// <summary>
    /// Interaction logic for PR_InputView.xaml
    /// </summary>
    public partial class PR_InputView : UserControl, IModuleViewBase
    {
        public PR_InputView()
        {
            InitializeComponent();
        }

        public string GetViewsDataContextAsPropertyNameOfModuleViewModel()
        {
            return "InputDescriptor";
        }
    }
}
