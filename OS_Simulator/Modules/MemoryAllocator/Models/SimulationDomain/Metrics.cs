using Simulator.Infrastructure;

namespace MemoryAllocator.Models
{
    public class Metrics : Notifier
    {
        public Metrics(MA_SimulatorModel simulatorModel)
        {
            this.simulatorModel = simulatorModel;
        }

        MA_SimulatorModel simulatorModel;

        public void Run()
        {
            OnPropertyChanged("MemoryUsage");
            OnPropertyChanged("RateOfUedAndAnusedSpaces");
            OnPropertyChanged("CountOfUsedPartitions");
            OnPropertyChanged("CountOfUnusedPartitions");
            OnPropertyChanged("RatioOfUsedAndAllPartitions");
            OnPropertyChanged("RatioOfUsedAndUnusedPartitions");
        }

        public float MemoryUsage
        {
            get 
            {
                int usedSpace = 0;
                int unusedSpace = 0;
                foreach (PartitionRecord record in simulatorModel.Partitions )
                {
                    if (record.PartitionType == PartitionType.Free)
                        unusedSpace += record.Size;
                    if (record.PartitionType == PartitionType.Used)
                        usedSpace += record.Size;
                }
                return (float)unusedSpace / (float)(unusedSpace+usedSpace);
            }
        }

        public float RateOfUedAndAnusedSpaces
        {
            get
            {
                int used = 0;
                int unused = 0;
                foreach (PartitionRecord record in simulatorModel.Partitions)
                {
                    if (record.PartitionType == PartitionType.Free)
                        unused += 1;
                    if (record.PartitionType == PartitionType.Used)
                        used += 1;
                }
                return (float)unused / (float)(unused + used);
            }
        }

        public int CountOfUsedPartitions
        {
            get
            {
                int used = 0;
                foreach (PartitionRecord record in simulatorModel.Partitions)
                {
                    if (record.PartitionType == PartitionType.Used)
                        used += 1;
                }
                return used;
            }
        }

        public int  CountOfUnusedPartitions
        {
            get
            {
                int unused = 0;
                foreach (PartitionRecord record in simulatorModel.Partitions)
                {
                    if (record.PartitionType == PartitionType.Free)
                        unused += 1;
                }
                return unused ;
            }
        }

        public float RatioOfUsedAndAllPartitions
        { 
            get
            {
                return (float)CountOfUsedPartitions / (float)(CountOfUnusedPartitions+CountOfUsedPartitions);
            }
        }

        public float  RatioOfUsedAndUnusedPartitions
        {
            get
            {
                return (float)CountOfUsedPartitions / (float)(CountOfUnusedPartitions);
            }
        }


    }
}
