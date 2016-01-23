using System.Collections.ObjectModel;
using System.Linq;

namespace TaskScheduler.Models
{
    public class FIFO : ITaskScheduler
    {
        public FIFO(TS_SimulatorModel simulatorModel, ObservableCollection<string> parameters)
        {
            this.simulatorModel = simulatorModel;
        }

        TS_SimulatorModel simulatorModel;

        public Process get()
        {
            Process longestWaiting = GetLongestWaitingReadyProcess();
            simulatorModel.Queue.Remove(longestWaiting);
            return longestWaiting;
        }

        public AlgorithmActingAndPreemptivityType GetActingAndPreemptivityType()
        {
            return AlgorithmActingAndPreemptivityType.None;
        }

        private Process GetLongestWaitingReadyProcess()
        {
            Process longestWaiting = simulatorModel.Queue.FirstOrDefault(x => x.ProcessStatus == ProcessStatusEnum.Ready);

            foreach (Process process in simulatorModel.Queue.Where(u => u.ProcessStatus == ProcessStatusEnum.Ready))
            {
                if (longestWaiting == null)
                {
                    return null;
                }
                else
                {
                    if (longestWaiting.TimeWaited < process.TimeWaited)
                    {
                        longestWaiting = process;
                    }
                }
            }
            return longestWaiting;
        }
    }
}
