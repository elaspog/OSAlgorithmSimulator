using PageReplacer.Models;
using Simulator.Infrastructure;
using Simulator.Infrastructure.Models;
using System;


namespace PageReplacer.ViewModels
{
    public class PR_SimulatorViewModel : Notifier, ISimulator
    {
        public PR_SimulatorViewModel()
        {
            simulatorModel = new PR_SimulatorModel();
        }

        private PR_SimulatorModel simulatorModel;
        public PR_SimulatorModel SimulatorModel
        {
            get { return simulatorModel; }
            set
            {
                simulatorModel = value;
                OnPropertyChanged("SimulatorModel");
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


        public bool NextStep()
        {
            bool result = SimulatorModel.NextStep();
            return result;
        }

        public bool PreviousStep()
        {
            int actualstep = simulatorModel.StepCounter;

            actualstep--;

            if (actualstep >= 0)
            {


                SimulatorModel = new PR_SimulatorModel();

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
