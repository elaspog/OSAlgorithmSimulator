using MemoryAllocator.Models;
using Simulator.Infrastructure;
using Simulator.Infrastructure.Models;
using System;

namespace MemoryAllocator.ViewModels
{
    public class MA_SimulatorViewModel : Notifier, ISimulator
    {
        public MA_SimulatorViewModel()
        {
            SimulatorModel = new MA_SimulatorModel();
        }

        private MA_SimulatorModel simulatorModel;
        public MA_SimulatorModel SimulatorModel
        {
            get { return simulatorModel; }
            set
            {
                simulatorModel = value;
                OnPropertyChanged("SimulatorModel");
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

        int step = 0;
        public bool NextStep()
        {
            bool result = SimulatorModel.NextStep();
            if (result)
            {
                step++;
            }

            return result;
        }

        public bool PreviousStep()
        {
            int actualstep = step;

            actualstep--;
            step = 0;

            if (actualstep >= 0)
            {
                SimulatorModel = new MA_SimulatorModel();

                try
                {
                    SimulatorModel.CreateSimulatorDomain(InputDescriptor);
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
