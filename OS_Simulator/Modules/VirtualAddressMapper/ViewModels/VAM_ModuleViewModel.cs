using Simulator.Infrastructure;
using Simulator.Infrastructure.Repository;
using Simulator.Infrastructure.ViewModels;
using System;
using System.Xml;
using System.Xml.Serialization;
using VirtualAddressMapper.Models;

namespace VirtualAddressMapper.ViewModels
{
    public class VAM_ModuleViewModel : Notifier, IModuleViewModelBaseFacade
    {
        public VAM_ModuleViewModel()
        {
            simulatorViewModel = new VAM_SimulatorViewModel();
        }

        private VAM_SimulatorViewModel simulatorViewModel;
        public VAM_SimulatorViewModel SimulatorViewModel
        {
            get { return simulatorViewModel; }
            set
            {
                simulatorViewModel = value;
                OnPropertyChanged("SimulatorViewModel");
            }
        }

        private VAM_Descriptor inputDescriptor;
        public VAM_Descriptor InputDescriptor
        {
            get { return inputDescriptor; }
            set
            {
                inputDescriptor = value;
                OnPropertyChanged("InputDescriptor");
            }
        }

        public Simulator.Infrastructure.Repository.SimulationStatus InicializeModuleByStream(System.IO.Stream XMLInputstream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(VAM_Descriptor));
            try
            {
                using (XmlReader reader = XmlReader.Create(XMLInputstream))
                {
                    InputDescriptor = (VAM_Descriptor)serializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                return SimulationStatus.Corrupted;
            }

            try
            {
                SimulatorViewModel.SimulatorModel.CreateSimulatorDomain(InputDescriptor);
                SimulatorViewModel.InputDescriptor = InputDescriptor;
            }
            catch (Exception e)
            {
                return SimulationStatus.Corrupted;
            }

            return SimulationStatus.Runnable;
        }

        public bool NextStep()
        {
            return SimulatorViewModel.NextStep();
        }

        public bool PreviousStep()
        {
            return SimulatorViewModel.PreviousStep();
        }

        public int GetActualCountOfStates()
        {
            throw new NotImplementedException();
        }

        public void ShowState(int i)
        {
            throw new NotImplementedException();
        }
    }
}
