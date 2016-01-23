using Simulator.Infrastructure;
using System.Xml.Serialization;

namespace MemoryAllocator.Models
{
    public class PhysicalMemory : Notifier
    {
        public PhysicalMemory()
        {

        }

        private int systemMemorySize;
        [XmlElementAttribute(ElementName = "SystemMemorySize")]
        public int SystemMemorySize
        {
            get { return systemMemorySize; }
            set
            {
                systemMemorySize = value;
                OnPropertyChanged("SystemMemorySize");
            }
        }

        private int userMemorySize;
        [XmlElementAttribute(ElementName = "UserMemorySize")]
        public int UserMemorySize
        {
            get { return userMemorySize; }
            set
            {
                userMemorySize = value;
                OnPropertyChanged("UserMemorySize");
            }
        }

        public int PhysicalMemorySize
        {
            get { return UserMemorySize + SystemMemorySize; }
        }

    }
}
