using Simulator.Infrastructure;
using System.Linq;

namespace TaskScheduler.Models
{
    public class Metrics : Notifier
    {
        public Metrics(TS_SimulatorModel simulatorModel, TS_Descriptor descriptor)
        {
            this.simulatorModel = simulatorModel;

            avarageOverheadTimeInOneThousandthMs = (double)descriptor.AvarageOverheadTime/1000.0;
            countOfMillisecondsWhereProcessWasRunning = countOfMillisecondsWhereProcessWasNotRunning = 0;
            countOfContextSwitchings = CountOfTaskSchedulings = 0;
        }


        private TS_SimulatorModel simulatorModel;
        
        private double avarageOverheadTimeInOneThousandthMs;
        private int countOfContextSwitchings;
        private int countOfMillisecondsWhereProcessWasRunning;
        private int countOfMillisecondsWhereProcessWasNotRunning;
        private int countOfTaskSchedulings;
        
        private int waitingTime;
        private int turnaroundTime;



        public void update()
        {
            OnPropertyChanged("AvarageOverheadTime");
            OnPropertyChanged("DegreeOfMultiProgramming");
            OnPropertyChanged("CpuUtilization");
            OnPropertyChanged("Throughput");
            OnPropertyChanged("WaitingTime");
            OnPropertyChanged("TurnaroundTime");
            OnPropertyChanged("CountOfContextSwitchings");
            OnPropertyChanged("CountOfMillisecondsWhereProcessWasRunning");
            OnPropertyChanged("CountOfMillisecondsWhereProcessWasNotRunning");
            OnPropertyChanged("CountOfTaskSchedulings");
        }


        public void run()
        {
            int readyProcesses = simulatorModel.ArrivedProcesses.Where(x => x.ProcessStatus == ProcessStatusEnum.Ready).Count();
            int waitingProcesses = simulatorModel.ArrivedProcesses.Where(x => x.ProcessStatus == ProcessStatusEnum.Waiting).Count();

            waitingTime += readyProcesses;
            waitingTime += waitingProcesses;
        }



        public double AvarageOverheadTimeInOneThousandthMs
        {
            get { return avarageOverheadTimeInOneThousandthMs; }
            set
            {
                avarageOverheadTimeInOneThousandthMs = value;
                OnPropertyChanged("AvarageOverheadTimeInOneThousandthMs");
            }
        }

        public int DegreeOfMultiProgramming
        {
            get
            {
                int readyProcesses      = simulatorModel.ArrivedProcesses.Where(x => x.ProcessStatus == ProcessStatusEnum.Ready).Count();
                int runningProcesses    = simulatorModel.ArrivedProcesses.Where(x => x.ProcessStatus == ProcessStatusEnum.Running).Count();
                int waitingProcesses    = simulatorModel.ArrivedProcesses.Where(x => x.ProcessStatus == ProcessStatusEnum.Waiting).Count();

                return readyProcesses + runningProcesses + waitingProcesses; 
            }
        }

        public float CpuUtilization
        {
            get 
            {
                return (((float)CountOfMillisecondsWhereProcessWasRunning) /
                    ((float)((CountOfMillisecondsWhereProcessWasRunning + CountOfMillisecondsWhereProcessWasNotRunning) 
                            + (((CountOfTaskSchedulings - CountOfContextSwitchings) + CountOfContextSwitchings) * AvarageOverheadTimeInOneThousandthMs))));
            }
        }

        public float Throughput
        {
            get
            {
                int finishedProcesses = simulatorModel.ArrivedProcesses.Where(x => x.ProcessStatus == ProcessStatusEnum.Finished).Count();
                return (float)finishedProcesses / (float)(CountOfMillisecondsWhereProcessWasRunning + CountOfMillisecondsWhereProcessWasNotRunning); 
            }
        }

        public float WaitingTime
        {
            get
            {
                return (float)waitingTime / simulatorModel.ArrivedProcesses.Count;
            }
        }

        public float TurnaroundTime
        {
            get
            {
                int finishedProcesses = simulatorModel.ArrivedProcesses.Where(x => x.ProcessStatus == ProcessStatusEnum.Finished).Count();

                return (float)turnaroundTime / (float)finishedProcesses; 
            }
        }
               

        public int CountOfContextSwitchings
        {
            get { return countOfContextSwitchings; }
            set
            {
                countOfContextSwitchings = value;
                OnPropertyChanged("CountOfContextSwitchings");
            }
        }

        public int CountOfMillisecondsWhereProcessWasRunning
        {
            get { return countOfMillisecondsWhereProcessWasRunning; }
            set
            {
                countOfMillisecondsWhereProcessWasRunning = value;
                OnPropertyChanged("CountOfMillisecondsWhereProcessWasRunning");
            }
        }

        public int CountOfMillisecondsWhereProcessWasNotRunning
        {
            get { return countOfMillisecondsWhereProcessWasNotRunning; }
            set
            {
                countOfMillisecondsWhereProcessWasNotRunning = value;
                OnPropertyChanged("CountOfMillisecondsWhereProcessWasNotRunning");
            }
        }

        public int CountOfTaskSchedulings
        {
            get { return countOfTaskSchedulings; }
            set
            {
                countOfTaskSchedulings = value;
                OnPropertyChanged("CountOfTaskSchedulings");
            }
        }
        
        public void addTurnaroundTime(int p)
        {
            turnaroundTime += p;
        }
    }
}
