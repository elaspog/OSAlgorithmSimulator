using System.Collections.ObjectModel;
using System.Linq;

namespace TaskScheduler.Models
{
    class SJF : ITaskScheduler
    {
        public SJF(TS_SimulatorModel simulatorModel, ObservableCollection<string> parameters)
        {
            this.simulatorModel = simulatorModel;
        }

        TS_SimulatorModel simulatorModel;

        public Process get()
        {
            Process shortestEstimatedBurstTime = GetShortestEstimatedBurstTimeReadyProcess();
            simulatorModel.Queue.Remove(shortestEstimatedBurstTime);
            return shortestEstimatedBurstTime;
        }

        public AlgorithmActingAndPreemptivityType GetActingAndPreemptivityType()
        {
            return AlgorithmActingAndPreemptivityType.None;
        }

        private Process GetShortestEstimatedBurstTimeReadyProcess()
        {
            Process shortestEstimatedBurstTime = simulatorModel.Queue.FirstOrDefault(x => x.ProcessStatus == ProcessStatusEnum.Ready);

            foreach (Process process in simulatorModel.Queue.Where(u => u.ProcessStatus == ProcessStatusEnum.Ready))
            {
                if (shortestEstimatedBurstTime == null)
                {
                    return null;
                }
                else
                {
                    if (shortestEstimatedBurstTime.TotalTimeLeft > process.TotalTimeLeft)
                    {
                        shortestEstimatedBurstTime = process;
                    }
                }
            }
            return shortestEstimatedBurstTime;
        }
    }
}
