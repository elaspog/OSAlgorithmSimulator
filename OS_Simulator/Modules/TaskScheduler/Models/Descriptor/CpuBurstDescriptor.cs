using System;
using System.Xml.Serialization;

namespace TaskScheduler.Models
{
    [Serializable]
    public class CpuBurstDescriptor : BurstDescriptor
    {
        public CpuBurstDescriptor()
            : base()
        { }

        public CpuBurstDescriptor(int burstTime)
            : base(burstTime)
        { }


        [XmlIgnore]
        public string BurstType
        {
            get { return "CPU Burst"; }
        }

        [XmlIgnore]
        public string ResourceName
        {
            get { return "CPU"; }
        }

    }
}
