using System.Collections.ObjectModel;
using System.Linq;

namespace TaskScheduler.Models
{
    class SRTF : ITaskSchedulerByEvent
    {
        public SRTF(TS_SimulatorModel simulatorModel, ObservableCollection<string> parameters)
        {
            this.simulatorModel = simulatorModel;
            savedQueue = new ObservableCollection<Process>();
        }

        TS_SimulatorModel simulatorModel;
        ObservableCollection<Process> savedQueue;

        public Process get()
        {
            Process runningProcess = simulatorModel.ArrivedProcesses.FirstOrDefault(x => x.ProcessStatus == ProcessStatusEnum.Running);
            if (runningProcess != null)
            {
                runningProcess.ProcessStatus = ProcessStatusEnum.Ready;
                simulatorModel.Queue.Add(runningProcess);
            }

            Process shortestEstimatedBurstTime = GetShortestEstimatedBurstTimeReadyProcess();
            simulatorModel.Queue.Remove(shortestEstimatedBurstTime);
            return shortestEstimatedBurstTime;
        }


        public AlgorithmActingAndPreemptivityType GetActingAndPreemptivityType()
        {
            return AlgorithmActingAndPreemptivityType.ByArrivedProcess;
        }

        private Process GetShortestEstimatedBurstTimeReadyProcess()
        {
            Process shortestEstimatedBurstTime = simulatorModel.ArrivedProcesses.FirstOrDefault(x => x.ProcessStatus == ProcessStatusEnum.Ready);

            foreach (Process process in simulatorModel.ArrivedProcesses.Where(u => u.ProcessStatus == ProcessStatusEnum.Ready))
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

        public void update()
        {
            savedQueue.Clear();
            foreach (Process process in simulatorModel.Queue)
            {
                savedQueue.Add(process);
            }
        }

        public bool needToRun()
        {
            if (savedQueue.Count != simulatorModel.Queue.Count)
            {
                return true;
            }
            for (int i = 0; i < savedQueue.Count; i++)
            {
                if (savedQueue.ElementAt(i) != simulatorModel.Queue.ElementAt(i))
                {
                    return true;
                }
            }
            return false;
            
        }
    }
}
