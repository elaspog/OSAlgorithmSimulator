using System;
using System.Xml.Serialization;

namespace TaskScheduler.Models
{
    [Serializable]
    public class IoBurstAsynchronousDescriptor : IoBurstDescriptor
    {
        public IoBurstAsynchronousDescriptor()
            : base()
        { }

        public IoBurstAsynchronousDescriptor(int burstTime, string resourceName)
            : base(burstTime, resourceName)
        { }

        [XmlIgnore]
        public string BurstType
        {
            get { return "IO Async"; }
        }

    }
}
