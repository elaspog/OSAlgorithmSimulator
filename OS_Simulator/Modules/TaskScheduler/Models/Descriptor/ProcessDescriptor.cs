using Simulator.Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace TaskScheduler.Models
{
    [Serializable]
    public class ProcessDescriptor : Notifier
    {
        public ProcessDescriptor() 
        {
            burstSequence = new ObservableCollection<BurstDescriptor>();
        }

        private string processName;
        [XmlAttribute("ProcessName")]
        public string ProcessName
        {
            get { return processName; }
            set
            {
                processName = value;
                OnPropertyChanged("ProcessName");
            }
        }

        private int arrivalTime;
        [XmlElementAttribute(ElementName = "ArrivalTime")]
        public int ArrivalTime
        {
            get { return arrivalTime; }
            set
            {
                arrivalTime = value;
                OnPropertyChanged("ArrivalTime");
            }
        }

        private ObservableCollection<BurstDescriptor> burstSequence;
        [XmlArrayItem("CpuBurst", Type = typeof(CpuBurstDescriptor))]
        [XmlArrayItem("IoBurstAsynchronous", Type = typeof(IoBurstAsynchronousDescriptor))]
        [XmlArrayItem("IoBurstSynchronous", Type = typeof(IoBurstSynchronousDescriptor))]
        public ObservableCollection<BurstDescriptor> BurstSequence
        {
            get { return burstSequence; }
            set
            {
                burstSequence = value;
                OnPropertyChanged("BurstSequence");
            }
        }

        [XmlIgnore]
        public int TotalBurstTime
        {
          get 
          {
              int sum = 0;
              foreach (BurstDescriptor burst in burstSequence)
              {
                  sum += burst.BurstTime;
              }
              return sum;
          }
        }
    }
}
