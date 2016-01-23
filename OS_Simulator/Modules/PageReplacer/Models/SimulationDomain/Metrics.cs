using Simulator.Infrastructure;

namespace PageReplacer.Models
{
    public class Metrics : Notifier
    {
        public Metrics( int memoryAccessTime, int pageFaultServiceTime)
        {
            this.memoryAccessTime = memoryAccessTime;
            this.pageFaultServiceTime = pageFaultServiceTime;
        }


        private int memoryAccessTime;
        public int MemoryAccessTime
        {
            get { return memoryAccessTime; }
            set
            {
                memoryAccessTime = value;
                OnPropertyChanged("MemoryAccessTime");
                OnPropertyChanged("AvarageAddressMappingTime");
            }
        }

        private int pageFaultServiceTime;
        public int PageFaultServiceTime
        {
            get { return pageFaultServiceTime; }
            set
            {
                pageFaultServiceTime = value;
                OnPropertyChanged("PageFaultServiceTime");
                OnPropertyChanged("AvarageAddressMappingTime");
            }
        }


        private int countOfMemoryAccesses;
        public int CountOfMemoryAccesses
        {
            get { return countOfMemoryAccesses; }
            set
            {
                countOfMemoryAccesses = value;
                OnPropertyChanged("CountOfMemoryAccesses");
                OnPropertyChanged("AvarageAddressMappingTime");
            }
        }


        private int countOfPageFaults;
        public int CountOfPageFaults
        {
            get { return countOfPageFaults; }
            set
            {
                countOfPageFaults = value;
                OnPropertyChanged("CountOfPageFaults");
                OnPropertyChanged("AvarageAddressMappingTime");
            }
        }

        public float AvarageAddressMappingTime
        {
            get
            { 
                return (((float)((CountOfPageFaults * PageFaultServiceTime) + (CountOfMemoryAccesses * MemoryAccessTime)))) / ((float)(CountOfPageFaults + CountOfMemoryAccesses));
            }
        }

        public void setMetrics(PageRecord pageRecord)
        {
            if (pageRecord.PageFault == true)
            {
                CountOfPageFaults++;
            }
            else
            {
                CountOfMemoryAccesses++;
            }
        }


    }
}
