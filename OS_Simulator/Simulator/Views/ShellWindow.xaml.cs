using GalaSoft.MvvmLight.Messaging;
using Simulator.Infrastructure.EventArgs;
using Simulator.Infrastructure.ViewModels;
using Simulator.Infrastructure.Views;
using Simulator.Services;
using Simulator.ViewModels;
using System;

using System.Windows;
using System.Windows.Controls;

namespace Simulator.Views
{
    /// <summary>
    /// Interaction logic for ShellWindow.xaml
    /// </summary>
    public partial class ShellWindow : Window
    {
        ShellViewModelSingletonContainer shellViewModelSingletonContainer = new ShellViewModelSingletonContainer();

        public ShellWindow(OpenSimulationMessage openSimulationMessage)
        {
            InitializeComponent();
            
            // Hibás lenne minden ablak létrehozásnál új viewmodel-t is létrehozni, mert mi van ha az már létezik, ezért singleton-ra bízzuk
            ShellViewModel shellViewModel = shellViewModelSingletonContainer.GetOrCreateShellViewModelInstance(openSimulationMessage.SimulationRecord);
            DataContext = shellViewModel;

            Messenger.Default.Register<RemoveSimulationMessage>(this, message =>
            {
                // Mivel már csak meglevő ablakoknál lehet, itt a GetExistingShellViewModelInstance Factory-jával már nem kell foglalkozni
                ShellViewModel shellViewModelToBeRemoved = shellViewModelSingletonContainer.GetExistingShellViewModelInstance(message.SimulationRecord);
                if (((ShellViewModel)this.DataContext).Equals(shellViewModelToBeRemoved))
                {
                    // ViewModelek és mások törlése
                    shellViewModelSingletonContainer.RemoveShellViewModelInstance(message.SimulationRecord);
                    Messenger.Default.Unregister(this);
                    this.Close();
                }

            });

            IOService ioService = new ModuleDependentIOService();
            Simulator.Infrastructure.Module moduleInstance = ioService.GetModuleInstanceBySimulationRecord(openSimulationMessage.SimulationRecord);

            // View típusok lekérése
            Type inputViewType      = moduleInstance.GetInputViewType();
            Type simulationViewType = moduleInstance.GetSimulationViewType();
            Type statisticsViewType = moduleInstance.GetStatisticsViewType();
            Type diagramViewType    = moduleInstance.GetDiagramVewType();
            Type logsViewType       = moduleInstance.GetLogsViewType();

            // ModuleView-k beállítása a megfelelő nevesített területekhez és DataContext-jük ellátása ModuleViewModel-el vagy azon belül egy a ModuleViewModel által kezelt ViewModel-lel
            setShellWindowAreas(shellViewModel.ModuleViewModel, inputViewType, "InputInformationView", "InputInformationTab");
            setShellWindowAreas(shellViewModel.ModuleViewModel, simulationViewType, "SimulationView", "SimulationTab");
            setShellWindowAreas(shellViewModel.ModuleViewModel, statisticsViewType, "StatisticsView", "StatisticsTab");
            setShellWindowAreas(shellViewModel.ModuleViewModel, diagramViewType, "DiagramsView", "DiagramsTab");
            setShellWindowAreas(shellViewModel.ModuleViewModel, logsViewType, "LogsView", "LogsTab");

        }


        public void setShellWindowAreas(IModuleViewModelBaseFacade moduleViewModel, Type viewType, string contentControlName, string tabControlName)
        {
            if (viewType != null)
            {
                // nézet csatolása
                var moduleView = Activator.CreateInstance(viewType);
                typeof(ShellWindow).GetMethod("AppendModuleViewToContentControl").MakeGenericMethod(viewType).Invoke(this, new[] { contentControlName, moduleView });

                // ha nem elég neki a ModuleViewModel DataContext-je és egy azon belül elhelyezett Property-t szeretne használni DataContext-ként
                IModuleViewBase view  = (IModuleViewBase)moduleView;

                // a view tól el kell kérni, hogy van-e a ModuleViewModel-en belül property, amelyet DataContext-ként szeretne használni           
                string viewsDataContextPropertyNameInModuleViewModel = view.GetViewsDataContextAsPropertyNameOfModuleViewModel();

                // ha van olyan Property, melyhez lehet csatolni és ha van ModuleViewModel objektum is (különben nem lenne objektum, melynek property-jeihez referenciát kelle állítani)
                if (viewsDataContextPropertyNameInModuleViewModel != null && (! viewsDataContextPropertyNameInModuleViewModel.Equals("")) && moduleViewModel != null)
                {
                    System.Reflection.PropertyInfo[] properties = moduleViewModel.GetType().GetProperties();
                    foreach (System.Reflection.PropertyInfo property in properties)
                    {
                        if (property.Name.Equals(viewsDataContextPropertyNameInModuleViewModel))
                        {
                            object dataContext = property.GetValue(moduleViewModel, null);
                            ((ContentControl)FindName(contentControlName)).DataContext = dataContext;
                            //((ContentControl)FindName(contentControlName)).DataContext = moduleViewModel.???;
                            break;
                        }
                    }
                } // ha nincs akkor a ModulViewModel-t veszi alapértelmezett DataContext-nek
                else
                {
                    ((ContentControl)FindName(contentControlName)).DataContext = moduleViewModel;
                }
            }
            else
            {
                ((TabItem)FindName(tabControlName)).Visibility = System.Windows.Visibility.Collapsed;
            }
        }


        public void AppendModuleViewToContentControl<T>(string contentControlXName, T view) 
            where T: UserControl
        {
            ((ContentControl)FindName(contentControlXName)).Content = view;
        }
    }
}
