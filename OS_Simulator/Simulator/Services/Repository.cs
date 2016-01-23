using GalaSoft.MvvmLight.Messaging;
using Simulator.Infrastructure.EventArgs;
using Simulator.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Simulator.Services
{
    public class Repository
    {
        private IOService ioService;

        private ObservableCollection<SimulationRecordWithModuleInfo> loadedSimulations = new ObservableCollection<SimulationRecordWithModuleInfo>();
        public ObservableCollection<SimulationRecordWithModuleInfo> LoadedSimulations
        {
            get { return loadedSimulations; }
            set { loadedSimulations = value; }

        }

        public Repository()
        { 
            ioService = new ModuleDependentIOService(); 
        }

        public List<SimulationRecordWithModuleInfo> LoadSimulations()
        {
            List<SimulationRecordWithModuleInfo> currentlyLoadedSimulations = new List<SimulationRecordWithModuleInfo>() ;

            string[] SelectedPaths = ioService.OpenFiles();

            if (SelectedPaths != null)
            {
                foreach (string SelectedPath in SelectedPaths)
                {
                    SimulationRecordWithModuleInfo simulationRecordToCheckAndAdd = ioService.ReadXmlHeader(SelectedPath);

                    if (simulationRecordToCheckAndAdd != null && simulationRecordToCheckAndAdd.SimulationType != null)
                    {

                        if (LoadedSimulations.Contains(simulationRecordToCheckAndAdd))
                        {
                            Messenger.Default.Send(new SendModalWindowMessage("This simulation with the same Title and Type is already loaded.", "Already has it"));
                        }
                        else
                        {
                            simulationRecordToCheckAndAdd.FileStream = ioService.GetFileStream(SelectedPath);

                            LoadedSimulations.Add(simulationRecordToCheckAndAdd);
                            currentlyLoadedSimulations.Add(simulationRecordToCheckAndAdd);
                        }
                    }
                    else
                    {
                        Messenger.Default.Send(new SendModalWindowMessage("Please make a reference betwen this type of task and the application.", "Unknown module"));
                    }
                }
            }
            return currentlyLoadedSimulations;
        }

        public void RemoveSimulation(List<SimulationRecordWithModuleInfo> selectedItems)
        {

            if (selectedItems != null)
            {
                foreach (SimulationRecordWithModuleInfo simDesrc in selectedItems)
                {
                    loadedSimulations.Remove(simDesrc);
                }
            }
        }

        public void ClearSimulationList()
        {
            loadedSimulations.Clear();
        }
    }
}
