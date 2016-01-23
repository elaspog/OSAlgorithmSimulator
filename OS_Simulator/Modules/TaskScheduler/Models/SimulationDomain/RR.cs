using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace TaskScheduler.Models
{
    class RR : ITaskSchedulerByTime
    {
        public RR(TS_SimulatorModel simulatorModel, ObservableCollection<string> parameters)
        {
            this.simulatorModel = simulatorModel;
            timeLimit = Convert.ToInt32(parameters.First());
            if (timeLimit <= 0)
            {
                timeLimit = 1;
            }
            actualTime = 0;
        }

        TS_SimulatorModel simulatorModel;
        private int timeLimit;
        private int actualTime;

        public Process get()
        {
            Process runningProcess = simulatorModel.ArrivedProcesses.FirstOrDefault(x => x.ProcessStatus == ProcessStatusEnum.Running);
            if (runningProcess != null)
            {
                runningProcess.ProcessStatus = ProcessStatusEnum.Ready;
                simulatorModel.Queue.Add(runningProcess);
            }

            Process selected = simulatorModel.Queue.FirstOrDefault(x => x.ProcessStatus == ProcessStatusEnum.Ready);
            simulatorModel.Queue.Remove(selected);

            actualTime = 0;
            return selected;
        }


        public AlgorithmActingAndPreemptivityType GetActingAndPreemptivityType()
        {
            return AlgorithmActingAndPreemptivityType.ByTimeSlice;
        }

        public void updateTime(int ms)
        {
            actualTime++;
        }

        public bool needToRun()
        {
            return timeLimit == actualTime;
        }
    }
}
