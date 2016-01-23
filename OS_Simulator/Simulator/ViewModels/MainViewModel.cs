using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using Simulator.Infrastructure.EventArgs;
using Simulator.Services;
using Simulator.Infrastructure.Repository;
using Simulator.Infrastructure.ViewModels;
using System.Windows.Threading;

namespace Simulator.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        ShellViewModelSingletonContainer shellViewModelSingletonContainer;

        public MainViewModel()
            : base()
        {
            shellViewModelSingletonContainer = new ShellViewModelSingletonContainer();
            StatusText = "Please load input file(s).";
            isAutomaticSimulation = false;
            AutomaticSimulationSpeed = 1;
        }

        Repository repository = new Repository();
        public Repository Repository
        {
            get { return repository; }
            set
            {
                repository = value;
                RaisePropertyChanged("Repository");
            }
        }
        
        private RelayCommand loadSimulationCommand;
        public RelayCommand LoadSimulationCommand
        {
            get { return loadSimulationCommand ?? (loadSimulationCommand = new RelayCommand(this.loadSimulation)); }
        }

        private RelayCommand removeSimulationCommand;
        public RelayCommand RemoveSimulationCommand
        {
            get { return removeSimulationCommand ?? (removeSimulationCommand = new RelayCommand(this.removeSimulation, this.isAtLeastOneRecordSelected)); }
        }

        private RelayCommand openSimulationCommand;
        public RelayCommand OpenSimulationCommand
        {
            get 
            {
                if (openSimulationCommand == null)
                    openSimulationCommand = new RelayCommand(sendOpenShellWindowMessage, isAtLeastOneRecordSelected);
                return openSimulationCommand;
            }
        }

        private RelayCommand clearSimulationListCommand;
        public RelayCommand ClearSimulationListCommand
        {
            get { return clearSimulationListCommand ?? (clearSimulationListCommand = new RelayCommand(this.clearSimulationList, this.canClearSimulationList)); }
        }

        private RelayCommand nextStepCommand;
        public RelayCommand NextStepCommand
        {
            get { return nextStepCommand ?? (nextStepCommand = new RelayCommand(this.sendNextStep, this.isManualControlEnabled)); }
        }

        private RelayCommand previousStepCommand;
        public RelayCommand PreviousStepCommand
        {
            get { return previousStepCommand ?? (previousStepCommand = new RelayCommand(this.sendPrevioustStep, this.isManualControlEnabled)); }
        }


        private RelayCommand startCommand;
        public RelayCommand StartCommand
        {
            get { return startCommand ?? (startCommand = new RelayCommand(this.StartAutomaticRun)); }
        }

        private RelayCommand pauseCommand;
        public RelayCommand PauseCommand
        {
            get { return pauseCommand ?? (pauseCommand = new RelayCommand(this.PauseAutomaticRun)); }
        }

        private RelayCommand helpCommand;
        public RelayCommand HelpCommand
        {
            get { return helpCommand ?? (helpCommand = new RelayCommand(this.openHelpWindow)); }
        }

        private void openHelpWindow()
        {
            Messenger.Default.Send(new OpenHelpWindowMessage());
        }

        private DispatcherTimer timer;
        private int _timesCalled = 0;

        private void PauseAutomaticRun()
        {
            timer.Stop();
        }

        private void StartAutomaticRun()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(AutomaticSimulationSpeed);
            timer.Tick += timer_Task;
            _timesCalled = 0;
            timer.Start();
        }

        private void timer_Task(object sender, EventArgs e)
        {
            _timesCalled++;
            sendNextStep();

        }
        
        private List<SimulationRecordWithModuleInfo> selectedItems = new List<SimulationRecordWithModuleInfo>();
        public List<SimulationRecordWithModuleInfo> SelectedItems { 
            get { return selectedItems; }
            set 
            { 
                selectedItems = value; 
                RaisePropertyChanged("SelectedItems");
            } 
        }


        
        private void sendNextStep()
        {
            if (SelectedItems != null)
            {
                foreach (SimulationRecordWithModuleInfo simulationRecord in SelectedItems)
                {
                    ShellViewModel shellViewModel = shellViewModelSingletonContainer.GetOrCreateShellViewModelInstance(simulationRecord);
                    shellViewModel.NextStep();
                }
            }
            StatusText = "Next state calculated.";
        }

        private void sendPrevioustStep()
        {
            if (SelectedItems != null)
            {
                foreach (SimulationRecordWithModuleInfo simulationRecord in SelectedItems)
                {
                    ShellViewModel shellViewModel = shellViewModelSingletonContainer.GetOrCreateShellViewModelInstance(simulationRecord);
                    shellViewModel.PreviousStep();
                }
            }
            StatusText = "Previous state restored.";
        }

        private void loadSimulation()
        {
            List<SimulationRecordWithModuleInfo> currentlyLoadedSimulations = repository.LoadSimulations();
            foreach (SimulationRecordWithModuleInfo loadedSimulation in currentlyLoadedSimulations)
            {
                ShellViewModel shellViewModel = shellViewModelSingletonContainer.GetOrCreateShellViewModelInstance(loadedSimulation);

                IOService ioService = new ModuleDependentIOService();
                Simulator.Infrastructure.Module moduleInstance = ioService.GetModuleInstanceBySimulationRecord(loadedSimulation);

                Type newModuleViewModel = moduleInstance.GetModuleViewModelType();
                if (newModuleViewModel != null)
                {
                    shellViewModel.ModuleViewModel = (IModuleViewModelBaseFacade)Activator.CreateInstance(newModuleViewModel);


                } // IF ELSE -> ha a modul nem kéri, akkor nem kerül létrehozásra ModuleViewModel, igaz anélkül valószínűleg semmit sem ér majd..

                if (shellViewModel.ModuleViewModel != null)
                {
                    loadedSimulation.Status = shellViewModel.ModuleViewModel.InicializeModuleByStream(loadedSimulation.FileStream);
                }
                StatusText = "File(s) loaded.";
            }
        }

        private void removeSimulation()
        {
            if (SelectedItems != null)
            {
                foreach (SimulationRecordWithModuleInfo simulationRecord in SelectedItems)
                {
                    Messenger.Default.Send(new RemoveSimulationMessage(simulationRecord, ""));
                }
            }
            repository.RemoveSimulation(SelectedItems);

            StatusText = "File(s) removed.";

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void sendOpenShellWindowMessage()
        {
            if (SelectedItems != null)
            {
                foreach (SimulationRecordWithModuleInfo simulationRecord in SelectedItems)
                {
                    Messenger.Default.Send(new OpenSimulationMessage(simulationRecord, ""));
                }
            }
        }

        private void clearSimulationList()
        {
            foreach (SimulationRecordWithModuleInfo simulationRecord in repository.LoadedSimulations)
            {
                Messenger.Default.Send(new RemoveSimulationMessage(simulationRecord, ""));
            }
            repository.ClearSimulationList();

            StatusText = "File list cleared. Please load file(s).";
        }

        private bool isManualControlEnabled()
        {
            return isAtLeastOneRecordSelected() && (!isAutomaticSimulation);
        }

        private bool isAtLeastOneRecordSelected()
        {
            return SelectedItems != null && SelectedItems.Count != 0;
        }

        private bool canClearSimulationList()
        {
            return repository.LoadedSimulations != null && repository.LoadedSimulations.Count != 0;
        }

        private string statusText;
        public string StatusText
        {
            get { return statusText; }
            set 
            { 
                statusText = value;
                RaisePropertyChanged("StatusText");
            }
        }

        private bool isAutomaticSimulation;
        public bool IsAutomaticSimulation
        {
            get { return isAutomaticSimulation; }
            set
            {
                isAutomaticSimulation = value;

                if (!value)
                {
                    timer.Stop();
                }

                RaisePropertyChanged("IsAutomaticSimulation");
                RaisePropertyChanged("InvertedIsAutomaticSimulation");
            }
        }

        private float automaticSimulationSpeed;
        public float AutomaticSimulationSpeed
        {
            get { return automaticSimulationSpeed; }
            set
            {
                automaticSimulationSpeed = value;

                if (timer != null)
                    timer.Interval = TimeSpan.FromSeconds(AutomaticSimulationSpeed);

                RaisePropertyChanged("AutomaticSimulationSpeed");
            }
        }

        public bool InvertedIsAutomaticSimulation
        {
            get { return !isAutomaticSimulation; }
            set
            {
                isAutomaticSimulation = ! value;
                RaisePropertyChanged("IsAutomaticSimulation");
                RaisePropertyChanged("InvertedIsAutomaticSimulation");
            }
        }

    }
}
