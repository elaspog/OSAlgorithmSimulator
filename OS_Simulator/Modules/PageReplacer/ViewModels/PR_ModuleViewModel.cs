using PageReplacer.Models;
using Simulator.Infrastructure;
using Simulator.Infrastructure.Repository;
using Simulator.Infrastructure.ViewModels;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace PageReplacer.ViewModels
{
    public class PR_ModuleViewModel : Notifier, IModuleViewModelBaseFacade
    {
        public PR_ModuleViewModel()
        {
            simulatorViewModel = new PR_SimulatorViewModel();
        }

        private PR_SimulatorViewModel simulatorViewModel;
        public PR_SimulatorViewModel SimulatorViewModel
        {
            get { return simulatorViewModel; }
            set
            {
                simulatorViewModel = value;
                OnPropertyChanged("SimulatorViewModel");
            }
        }

        private PR_Descriptor inputDescriptor;
        public PR_Descriptor InputDescriptor
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
            XmlSerializer serializer = new XmlSerializer(typeof(PR_Descriptor));
            try
            {
                using (XmlReader reader = XmlReader.Create(XMLInputstream))
                {
                    InputDescriptor = (PR_Descriptor)serializer.Deserialize(reader);
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
            bool res = SimulatorViewModel.NextStep();
            GC.Collect();
            return res;
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
