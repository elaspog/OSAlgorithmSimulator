using Simulator.Infrastructure;
using Simulator.Infrastructure.Repository;
using Simulator.Infrastructure.ViewModels;
using System;
using System.Xml;
using System.Xml.Serialization;
using TaskScheduler.Models;

namespace TaskScheduler.ViewModels
{
    public class TS_ModuleViewModel : Notifier, IModuleViewModelBaseFacade
    {
        public TS_ModuleViewModel()
        {
            simulatorViewModel = new TS_SimulatorViewModel();
            inputDescriptor = null;
        }

        private TS_SimulatorViewModel simulatorViewModel;
        public TS_SimulatorViewModel SimulatorViewModel
        {
            get { return simulatorViewModel; }
            set
            {
                simulatorViewModel = value;
                OnPropertyChanged("SimulatorViewModel");
            }
        }

        private TS_Descriptor inputDescriptor;
        public TS_Descriptor InputDescriptor
        {
            get { return inputDescriptor; }
            set
            {
                inputDescriptor = value;
                OnPropertyChanged("InputDescriptor");
            }
        }



        public bool NextStep()
        {
            SimulatorViewModel.NextStep();
            return true;
        }

        public bool PreviousStep()
        {
            SimulatorViewModel.PreviousStep();
            return true;
        }

        public SimulationStatus InicializeModuleByStream(System.IO.Stream XMLInputstream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TS_Descriptor));
            try
            {
                using (XmlReader reader = XmlReader.Create(XMLInputstream))
                {
                    InputDescriptor = (TS_Descriptor)serializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                return SimulationStatus.Corrupted;
            }

            try
            {
                SimulatorViewModel.SimulatorModel = new TS_SimulatorModel();
                SimulatorViewModel.SimulatorModel.CreateSimulatorDomain(InputDescriptor);
                SimulatorViewModel.InputDescriptor = InputDescriptor;
            }
            catch (Exception e)
            {
                return SimulationStatus.Corrupted;
            }


            return SimulationStatus.Runnable;
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
