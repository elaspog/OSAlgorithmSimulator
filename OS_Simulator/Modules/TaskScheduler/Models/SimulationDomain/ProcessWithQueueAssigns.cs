using Simulator.Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace TaskScheduler.Models
{
    public class ProcessWithQueueAssigns : Notifier
    {
        public ProcessWithQueueAssigns(string _processName, ObservableCollection<QueueAssign> _partTimes)
        {
            PartTimes = new ObservableCollection<QueueAssign>();
            ProcessName = _processName;
            PartTimes = _partTimes;
        }


        private string processName;
        public string ProcessName
        {
            get { return processName; }
            set
            {
                processName = value;
                OnPropertyChanged("ProcessName");
            }
        }


        private ObservableCollection<QueueAssign> partTimes;
        public ObservableCollection<QueueAssign> PartTimes
        {
            get { return partTimes; }
            set
            {
                partTimes = value;
                OnPropertyChanged("PartTimes");
                OnPropertyChanged("PartTimeSum");
            }
        }

        public int PartTimeSum
        {
            get
            {
                int sum = 0;
                if (PartTimes != null && PartTimes.Count > 0)
                {
                    foreach (QueueAssign queassign in PartTimes)
                    {
                        if (queassign != null)
                        {
                            sum += Math.Abs(queassign.IntervalEnd - queassign.IntervalStart);
                        }
                    }
                }
                return sum;
            }
        }

        public void setLastPartTimesEndInterval(int time)
        {
            PartTimes.Last().IntervalEnd = time;

            OnPropertyChanged("PartTimes");
            OnPropertyChanged("PartTimeSum");
        }

        public void addNewPartTime(QueueAssign partTime)
        {
            PartTimes.Add(partTime);

            OnPropertyChanged("PartTimes");
            OnPropertyChanged("PartTimeSum");
        }
    }
}
