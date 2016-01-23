using Simulator.Infrastructure;

namespace VirtualAddressMapper.Models
{
    public class Metrics : Notifier
    {
        public Metrics(int _associativeTime, int _memoryTime)
        {
            memoryTime = _memoryTime;
            associativeTime = _associativeTime;

            associativeHits = pageTableHits = memoryAccesses = 0;
        }

        private int associativeTime;
        private int memoryTime;


        private int associativeHits;
        public int AssociativeHits
        {
            get { return associativeHits; }
            set
            {
                associativeHits = value;
                OnPropertyChanged("AssociativeHits");
            }
        }

        private int memoryAccesses;
        public int MemoryAccesses
        {
            get { return memoryAccesses; }
            set
            {
                memoryAccesses = value;
                OnPropertyChanged("MemoryAccesses");
                OnPropertyChanged("EffectiveMemoryAccess");
            }
        }

        private int pageTableHits;
        public int PageTableHits
        {
            get { return pageTableHits; }
            set
            {
                pageTableHits = value;
                OnPropertyChanged("PageTableHits");
                OnPropertyChanged("EffectiveMemoryAccess");
            }
        }
        
        public void makePageTableAndAddressHit()
        {
            MemoryAccesses++;
            MemoryAccesses++;
            PageTableHits++;
            OnPropertyChanged("MemoryAccesses");
            OnPropertyChanged("PageTableHits");
            OnPropertyChanged("EffectiveMemoryAccess");
        }

        public void makeAssociativeMemoryHit()
        {
            AssociativeHits++;
            OnPropertyChanged("AssociativeHits");
            OnPropertyChanged("EffectiveMemoryAccess");
        }

        public float EffectiveMemoryAccess
        {
            get 
            {
                return ((float)((associativeHits * (associativeTime + memoryTime)) + (pageTableHits * 2 * memoryTime)) / (float)(associativeHits + pageTableHits));
            }
        }

    }
}
