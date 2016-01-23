using Simulator.Infrastructure;
using System.Xml.Serialization;

namespace MemoryAllocator.Models
{
    public abstract class PartitionBase : Notifier
    {
        public PartitionBase()
        {

        }

        private int size;
        [XmlAttribute("Size")]
        public int Size
        {
            get { return size; }
            set
            {
                size = value;
                OnPropertyChanged("Size");
            }
        }

    }
}
