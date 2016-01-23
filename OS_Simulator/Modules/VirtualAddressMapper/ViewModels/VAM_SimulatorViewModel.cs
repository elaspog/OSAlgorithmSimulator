using Simulator.Infrastructure;
using Simulator.Infrastructure.Models;
using System;
using VirtualAddressMapper.Models;

namespace VirtualAddressMapper.ViewModels
{
    public enum NumeralSystem
    { 
        Binary, Octal, Decimal, HexaDecimal
    }

    public class VAM_SimulatorViewModel : Notifier, ISimulator
    {
        public VAM_SimulatorViewModel()
        {
            simulatorModel = new VAM_SimulatorModel();
        }

        private VAM_SimulatorModel simulatorModel;
        public VAM_SimulatorModel SimulatorModel
        {
            get { return simulatorModel; }
            set
            {
                simulatorModel = value;
                OnPropertyChanged("SimulatorModel");
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

        private NumeralSystem addressNumeralSystem;
        public NumeralSystem AddressNumeralSystem
        {
            get { return addressNumeralSystem; }
            set
            {
                addressNumeralSystem = value;
                OnPropertyChanged("AddressNumeralSystem");
            }
        }
        
        public bool NextStep()
        {
            bool result = SimulatorModel.NextStep();
            return result;
        }


        public bool PreviousStep()
        {
            int actualstep = SimulatorModel.StepCounter;

            actualstep--;

            if (actualstep >= 0)
            {

                try
                {
                    SimulatorModel = new VAM_SimulatorModel(InputDescriptor);
                    for (int i = 0; i < actualstep; i++)
                    {
                        NextStep();
                    }
                }
                catch (Exception e)
                {

                }
            }

            GC.Collect();

            return true;
        }

    }
}
