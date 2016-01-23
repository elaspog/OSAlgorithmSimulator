using System.Xml.Serialization;

namespace MemoryAllocator.Models
{
    public class FreeAction : AllocationActionBase
    {
        public FreeAction()
        {

        }


        private int existingId;
        [XmlAttribute("ExistingId")]
        public int ExistingId
        {
            get { return existingId; }
            set
            {
                existingId = value;
                OnPropertyChanged("ExistingId");
            }
        }
    }
}
