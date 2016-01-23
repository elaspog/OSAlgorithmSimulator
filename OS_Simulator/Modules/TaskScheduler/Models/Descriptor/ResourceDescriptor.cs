using Simulator.Infrastructure;
using System;
using System.Xml.Serialization;

namespace TaskScheduler.Models
{
    [Serializable]
    public class ResourceDescriptor : Notifier, ICloneable
    {
        public ResourceDescriptor()
        {
        }

        public ResourceDescriptor(string _resourceName, int _quantity)
        {
            resourceName = _resourceName;
            quantity = _quantity;
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

        private int quantity;
        [XmlAttribute(AttributeName = "Quantity")]
        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        public object Clone()
        {
            return (ResourceDescriptor)this.MemberwiseClone();
        }
    }
}
