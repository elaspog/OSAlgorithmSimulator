using Simulator.Infrastructure;
using System;
using System.Xml.Serialization;

namespace TaskScheduler.Models
{
    [Serializable]
    public abstract class BurstDescriptor : Notifier, ICloneable
    {
        public BurstDescriptor()
        {
        }

        public BurstDescriptor(int _burstTime)
        {
            burstTime = _burstTime;
        }

        private int burstTime;
        [XmlAttribute("BurstTime")]
        public int BurstTime
        {
            get { return burstTime; }
            set 
            { 
                burstTime = value;
                OnPropertyChanged("BurstTime");
            }
        }
        
        public object Clone()
        {
            return (BurstDescriptor)this.MemberwiseClone();
        }

    }
}
