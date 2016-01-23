using MemoryAllocator.Models;
using Simulator.Infrastructure;
using Simulator.Infrastructure.Repository;
using Simulator.Infrastructure.ViewModels;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace MemoryAllocator.ViewModels
{
    public class MA_ModuleViewModel : Notifier,  IModuleViewModelBaseFacade
    {
        public MA_ModuleViewModel()
        {
            simulatorViewModel = new MA_SimulatorViewModel();
        }

        private MA_SimulatorViewModel simulatorViewModel;
        public MA_SimulatorViewModel SimulatorViewModel
        {
            get { return simulatorViewModel; }
            set
            {
                simulatorViewModel = value;
                OnPropertyChanged("SimulatorViewModel");
            }
        }

        private MA_Descriptor inputDescriptor;
        public MA_Descriptor InputDescriptor
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

            XmlSerializer serializer = new XmlSerializer(typeof(MA_Descriptor));
            try
            {
                using (XmlReader reader = XmlReader.Create(XMLInputstream))
                {
                    InputDescriptor = (MA_Descriptor)serializer.Deserialize(reader);
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
