using System.Xml.Serialization;

namespace MemoryAllocator.Models
{
    public class AllocateAction : AllocationActionBase
    {
        public AllocateAction()
        {

        }

        public AllocateAction(int requiredSize)
            : base()
        {
            this.requiredSize = requiredSize;
        }



        private int allocatedNewSpaceId;
        [XmlAttribute("AllocatedNewSpaceId")]
        public int AllocatedNewSpaceId
        {
            get { return allocatedNewSpaceId; }
            set
            {
                allocatedNewSpaceId = value;
                OnPropertyChanged("AllocatedNewSpaceId");
            }
        }
        
        private int requiredSize;
        [XmlAttribute("RequiredSize")]
        public int RequiredSize
        {
            get { return requiredSize; }
            set
            {
                requiredSize = value;
                OnPropertyChanged("RequiredSize");
            }
        }

    }
}
