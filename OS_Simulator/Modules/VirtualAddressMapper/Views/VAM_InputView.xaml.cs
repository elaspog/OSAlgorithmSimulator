using Simulator.Infrastructure.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VirtualAddressMapper.Views
{
    /// <summary>
    /// Interaction logic for VAM_InputView.xaml
    /// </summary>
    public partial class VAM_InputView : UserControl, IModuleViewBase
    {
        public VAM_InputView()
        {
            InitializeComponent();
        }

        public string GetViewsDataContextAsPropertyNameOfModuleViewModel()
        {
            return "InputDescriptor";
        }

    }
}
