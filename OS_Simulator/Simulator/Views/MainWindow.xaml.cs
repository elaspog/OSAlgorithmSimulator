using GalaSoft.MvvmLight.Messaging;
using Simulator.Infrastructure.EventArgs;
using Simulator.Infrastructure.Repository;
using Simulator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Simulator.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ShellViewModelSingletonContainer shellViewModelSingletonContainer = new ShellViewModelSingletonContainer(); 

            Messenger.Default.Register<OpenSimulationMessage>(this, message =>
            {
                openShellWindow(message);
            });

            Messenger.Default.Register<SendModalWindowMessage>(this, message =>
            {
                System.Windows.MessageBox.Show(((SendModalWindowMessage)message).Header, ((SendModalWindowMessage)message).Content);
            });


            Messenger.Default.Register<OpenHelpWindowMessage>(this, message =>
            {
                openHelpWindow();
            });

            this.Closed += MainWindow_Closed;
        }

        private void openHelpWindow()
        {
            HelpWindow helpWindow = new HelpWindow();

            helpWindow.Show();
        }

        void MainWindow_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        private void openShellWindow(OpenSimulationMessage message)
        {
            ShellWindow shellwindow = new ShellWindow(message);
            shellwindow.Show();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<SimulationRecordWithModuleInfo> selectedItems = new List<SimulationRecordWithModuleInfo>();
            foreach (SimulationRecordWithModuleInfo item in selectSimulator.simulationListView.SelectedItems)
            {
                selectedItems.Add(item);
            }
            ((MainViewModel)DataContext).SelectedItems = selectedItems;
        }


    }
}
