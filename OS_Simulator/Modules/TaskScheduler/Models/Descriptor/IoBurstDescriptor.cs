using System;
using System.Xml.Serialization;

namespace TaskScheduler.Models
{
    [Serializable]
    public abstract class IoBurstDescriptor : BurstDescriptor
    {
        public IoBurstDescriptor()
            : base()
        { }

        public IoBurstDescriptor(int burstTime, string resourceName)
            : base(burstTime)
        {
            this.resourceName = resourceName;
        }

        private string resourceName;
        [XmlAttribute(AttributeName = "ResourceName")]
        public string ResourceName
        {
            get { return resourceName; }
            set
            {
                resourceName = value;
                OnPropertyChanged("ResourceName");
            }
        }
        

    }
}
