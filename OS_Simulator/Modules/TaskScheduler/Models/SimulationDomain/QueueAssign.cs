using Simulator.Infrastructure;
using System;
using System.Collections.ObjectModel;

namespace TaskScheduler.Models
{
    public class QueueAssign : Notifier
    {
        public QueueAssign(int _intervalStart, int _intervalEnd, ObservableCollection<string> _queue)
        {
            queue = new ObservableCollection<string>();

            IntervalStart = _intervalStart;
            IntervalEnd = _intervalEnd;
            Queue = _queue;
        }


        private int intervalStart;
        private int intervalEnd;
        private ObservableCollection<string> queue;

        public int IntervalStart
        {
            get { return intervalStart; }
            set
            {
                intervalStart = value;
                OnPropertyChanged("IntervalStart");
            }
        }

        public int IntervalEnd
        {
            get { return intervalEnd; }
            set
            {
                intervalEnd = value;
                OnPropertyChanged("IntervalEnd");
                OnPropertyChanged("Difference");
                OnPropertyChanged("PartTimes");
                OnPropertyChanged("PartTimeSum");
            }
        }

        public ObservableCollection<string> Queue
        {
            get 
            {
                return queue; 
            }
            set
            {
                queue = value;
                OnPropertyChanged("Queue");
            }
        }

        public int Difference
        {
            get
            {
                return Math.Abs(IntervalEnd - intervalStart);
            }
        }

    }
}
