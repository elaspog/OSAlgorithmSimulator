using Simulator.Infrastructure;
using Simulator.Infrastructure.Models;
using System;
using TaskScheduler.Models;

namespace TaskScheduler.ViewModels
{
    public class TS_SimulatorViewModel : Notifier, ISimulator
    {
        public TS_SimulatorViewModel()
        {
            step = 0;
        }

        private TS_SimulatorModel simulatorModel;
        public TS_SimulatorModel SimulatorModel
        {
            get { return simulatorModel; }
            set
            {
                simulatorModel = value;
                OnPropertyChanged("SimulatorModel");
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

        
        public int Mytext
        {
            get { return Step; }
        }


        int step = 0;

        public int Step
        {
            get { return step; }
            set 
            {
                step = value;
                OnPropertyChanged("Mytext");
            }
        }

        public bool NextStep()
        {
            bool result = SimulatorModel.NextStep();
            if (result)
            {
                Step++;
                //Mytext += 1;
            }

            return result;
        }

        public bool PreviousStep()
        {
            int actualstep = Step;

            actualstep--;
            Step = 0;

            if (actualstep >= 0)
            {
                SimulatorModel = new TS_SimulatorModel();

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
