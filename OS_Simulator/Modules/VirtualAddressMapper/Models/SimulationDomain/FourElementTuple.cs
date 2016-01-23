using Simulator.Infrastructure;

namespace VirtualAddressMapper.Models
{
    public class FourElementTuple : Notifier
    {
        public FourElementTuple(int _pageAddress, int _frameAddress, MappingRecordOfProcessAndMemory _mappingRecordOfProcessAndMemory)
        {
            PageAddress = _pageAddress;
            FrameAddress = _frameAddress;
            MappingRecordOfProcessAndMemory = _mappingRecordOfProcessAndMemory;
        }


        private int pageAddress;
        private int frameAddress;
        private MappingRecordOfProcessAndMemory mappingRecordOfProcessAndMemory;


        public int PageAddress
        {
            get { return pageAddress; }
            set
            {
                pageAddress = value;
                OnPropertyChanged("PageAddress");
            }
        }
        public int FrameAddress
        {
            get { return frameAddress; }
            set
            {
                frameAddress = value;
                OnPropertyChanged("FrameAddress");
            }
        }
        public MappingRecordOfProcessAndMemory MappingRecordOfProcessAndMemory
        {
            get { return mappingRecordOfProcessAndMemory; }
            set 
            { 
                mappingRecordOfProcessAndMemory = value;
                OnPropertyChanged("MappingRecordOfProcessAndMemory"); 
            }
        }
        
    }
}
