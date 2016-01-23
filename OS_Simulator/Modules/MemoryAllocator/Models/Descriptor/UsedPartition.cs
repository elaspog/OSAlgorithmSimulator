using System.Xml.Serialization;

namespace MemoryAllocator.Models
{
    public class UsedPartition : PartitionBase
    {
        public UsedPartition()
        {

        }

        private int id;
        [XmlAttribute("Id")]
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
    }
}
