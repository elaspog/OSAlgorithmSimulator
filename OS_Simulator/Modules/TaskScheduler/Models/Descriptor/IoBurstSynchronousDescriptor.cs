using System;
using System.Xml.Serialization;

namespace TaskScheduler.Models
{
    [Serializable]
    public class IoBurstSynchronousDescriptor : IoBurstDescriptor
    {
        public IoBurstSynchronousDescriptor()
            : base()
        { }

        public IoBurstSynchronousDescriptor(int burstTime, string resourceName)
            : base(burstTime, resourceName)
        { }

        [XmlIgnore]
        public string BurstType
        {
            get { return "IO Sync"; }
        }

    }
}
