using GalaSoft.MvvmLight;
using Simulator.Infrastructure.Repository;
using System.Collections.ObjectModel;


namespace Simulator.ViewModels
{
    public class ShellViewModelSingletonContainer : ViewModelBase
    {
        public ShellViewModelSingletonContainer()
        {
        }

        private static ObservableCollection<FourElementTuple> executableableSimulationContainer = new ObservableCollection<FourElementTuple>();
        public ObservableCollection<FourElementTuple> ExecutableableSimulationContainer
        {
            get { return executableableSimulationContainer; }
        }

        public class FourElementTuple
        {
            public FourElementTuple(SimulationRecordWithModuleInfo simulationRecordWithModuleInfo, ShellViewModel shellViewModel)
            {
                this.simulationRecordWithModuleInfo = simulationRecordWithModuleInfo;
                this.shellViewModel = shellViewModel;
                this.countOfReferences = 0;
            }

            SimulationRecordWithModuleInfo simulationRecordWithModuleInfo;
            public SimulationRecordWithModuleInfo SimulationRecordWithModuleInfo
            {
                get { return simulationRecordWithModuleInfo; }
                set { simulationRecordWithModuleInfo = value; }
            }

            ShellViewModel shellViewModel;
            public ShellViewModel ShellViewModel
            {
                get { return shellViewModel; }
                set { shellViewModel = value; }
            }
            
            int countOfReferences;
            public int CountOfReferences
            {
                get { return countOfReferences; }
            }

            // SimulationRecord van egyedi azonosítóként használva
            public override bool Equals(object obj)
            {
                return this.SimulationRecordWithModuleInfo.Equals(((FourElementTuple)obj).SimulationRecordWithModuleInfo);
            }

            public void IncreaseCountOfReferences()
            {
                countOfReferences++;
            }

            public void DecreaseCountOfReferences()
            {
                if (countOfReferences >= 0)
                {
                    countOfReferences--;
                }
            }

        }

        public ShellViewModel GetExistingShellViewModelInstance(SimulationRecordWithModuleInfo simulationRecord)
        {
            FourElementTuple newTuple = new FourElementTuple(simulationRecord, new ShellViewModel(simulationRecord));

            foreach (FourElementTuple listElement in ExecutableableSimulationContainer)
            {
                if (listElement.Equals(newTuple))
                {
                    return listElement.ShellViewModel;
                }
            }
            return null;
        }

        public ShellViewModel GetOrCreateShellViewModelInstance(SimulationRecordWithModuleInfo simulationRecord)
        {
            ShellViewModel shellViewModel = GetExistingShellViewModelInstance(simulationRecord);

            if (shellViewModel == null)
            {
                FourElementTuple newTuple = new FourElementTuple(simulationRecord, new ShellViewModel(simulationRecord));
                ExecutableableSimulationContainer.Add(newTuple);
                shellViewModel = newTuple.ShellViewModel;
            }
            GetTupleBySimulationRecord(simulationRecord).IncreaseCountOfReferences();

            return shellViewModel;
        }

        public void RemoveShellViewModelInstance(SimulationRecordWithModuleInfo simulationRecord)
        {
            FourElementTuple newTuple = GetTupleBySimulationRecord(simulationRecord);

            if (newTuple != null)
            {
                if (newTuple.CountOfReferences == 1)
                {
                    newTuple.DecreaseCountOfReferences();
                    ExecutableableSimulationContainer.Remove(newTuple);
                }
                else
                {
                    newTuple.DecreaseCountOfReferences();
                }
            }
        }

        private FourElementTuple GetTupleBySimulationRecord(SimulationRecordWithModuleInfo simulationRecord)
        {
            FourElementTuple newTuple = new FourElementTuple(simulationRecord, new ShellViewModel(simulationRecord));

            foreach (FourElementTuple listElement in ExecutableableSimulationContainer)
            {
                if (listElement.Equals(newTuple))
                {
                    return listElement;
                }
            }
            return null;
        }

    }
}
