using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Simulator.Infrastructure.EventArgs;
using Simulator.Infrastructure.Repository;
using Simulator.Infrastructure.ViewModels;
using System;

namespace Simulator.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        public ShellViewModel(SimulationRecordWithModuleInfo simulationRecord)
        {
            this.simulationRecord = simulationRecord;
        }

        private SimulationRecordWithModuleInfo simulationRecord;
        public SimulationRecordWithModuleInfo SimulationRecord
        {
            get { return simulationRecord; }
            set { simulationRecord = value; }
        }

        private IModuleViewModelBaseFacade moduleViewModel;
        public IModuleViewModelBaseFacade ModuleViewModel
        {
            get { return moduleViewModel; }
            set { moduleViewModel = value; }
        }

        public bool NextStep()
        {
            if (ModuleViewModel != null)
            {
                ModuleViewModel.NextStep();
                return true;
            }
            else
            {
                sendErrorMessage();
                return false;
            }
        }

        public bool PreviousStep()
        {
            if (ModuleViewModel != null)
            {
                ModuleViewModel.PreviousStep();
                return true;
            }
            else
            {
                sendErrorMessage();
                return false;
            }
        }

        private void sendErrorMessage()
        {
            Messenger.Default.Send(new SendModalWindowMessage("Module is not implementing required interfaces. "
                + "\nIModuleViewModelBaseFacade is not implemented by type returned by Module.GetModuleViewModelType().", "Bad module"));
        }



        public void ShowState(int i)
        {
            throw new NotImplementedException();
        }

        public int GetActualCountOfStates()
        {
            throw new NotImplementedException();
        }
    }
}
