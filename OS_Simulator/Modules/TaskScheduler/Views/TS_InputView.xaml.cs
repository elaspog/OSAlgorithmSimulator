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
using TaskScheduler.Models;
using TaskScheduler.ViewModels;
using TaskScheduler.Views.UserControls;

namespace TaskScheduler.Views
{
    /// <summary>
    /// Interaction logic for InputView.xaml
    /// </summary>
    public partial class TS_InputView : UserControl, IModuleViewBase
    {
        public TS_InputView()
        {
            InitializeComponent();
            DataContextChanged += new DependencyPropertyChangedEventHandler(InputView_DataContextChanged);
        }


        private void InputView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext != null && this.DataContext.GetType() == typeof(TS_Descriptor))
            {
                TS_Descriptor descriptor = (TS_Descriptor)this.DataContext;
                foreach (ProcessDescriptor process in descriptor.Processes)
                {
                    InputProcessInfo processInfo = new InputProcessInfo();
                    processInfo.DataContext = process;
                    processInfo.Margin = new Thickness(10, 0, 0, 0);

                    taskRow.Children.Add(processInfo);
                }
            }
        }

        public string GetViewsDataContextAsPropertyNameOfModuleViewModel()
        {
            return "InputDescriptor";
        }
    }
}
