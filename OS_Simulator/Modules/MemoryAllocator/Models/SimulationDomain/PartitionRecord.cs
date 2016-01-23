using Simulator.Infrastructure;
using System.Collections.ObjectModel;


namespace MemoryAllocator.Models
{
    public enum PartitionType { Free, Used };

    public class PartitionRecord : Notifier
    {
        public PartitionRecord(int size, int id)
        {
            ids = new ObservableCollection<int>();
            this.size = size;
            this.ids.Add(id);
            this.partitionType = PartitionType.Used;
        }

        public PartitionRecord(int size)
        {
            ids = new ObservableCollection<int>();
            this.size = size;
            this.partitionType = PartitionType.Free;
        }

        private PartitionType partitionType;
        public PartitionType PartitionType
        {
            get { return partitionType; }
            set
            {
                partitionType = value;
                OnPropertyChanged("PartitionType");
            }
        }

        ObservableCollection<int> ids = new ObservableCollection<int>();
        public ObservableCollection<int> Ids
        {
            get { return ids; }
            set
            {
                ids = value;
                OnPropertyChanged("Partitions");
            }
        }

        private int size;
        public int Size
        {
            get { return size; }
            set
            {
                size = value;
                OnPropertyChanged("Size");
            }
        }

        public bool hasId(int _id)
        {
            foreach (int id in ids)
            {
                if (id == _id)
                    return true;
            }
            return false;
        }

        public void addId(int _id)
        {
            Ids.Add(_id);
        }

        public void mergeFreePartitions(int size)
        {
            this.Size += size;
        }

    }
}
