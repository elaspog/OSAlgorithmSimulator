using System.Linq;

namespace MemoryAllocator.Models
{
    public class NextFitAlgorithm : IAllocator
    {
        public NextFitAlgorithm(MA_SimulatorModel simulatorModel)
        {
            this.simulatorModel = simulatorModel;
        }

        MA_SimulatorModel simulatorModel;
        int LastIndex = 0;
        int lastGeneratedUserMemoryAddress = 0;

        public int LastGeneratedUserMemoryAddress
        {
            get { return lastGeneratedUserMemoryAddress; }
            set { lastGeneratedUserMemoryAddress = value; }
        }


        public PartitionRecord getPartition(AllocateAction allocation)
        {
            int PartitionsCount = simulatorModel.Partitions.Count;

            LastIndex = simulatorModel.Partitions.IndexOf(getPartitionByMemoryAddress(lastGeneratedUserMemoryAddress));

            if (LastIndex > PartitionsCount || LastIndex < 0)
                LastIndex = 0;

            for (int i = LastIndex; i < PartitionsCount; i++)
            {
                if (simulatorModel.Partitions.ElementAt(i).PartitionType == PartitionType.Free)
                {
                    if (simulatorModel.Partitions.ElementAt(i).Size >= allocation.RequiredSize)
                    {
                        LastGeneratedUserMemoryAddress = generateUserMemoryAddress(simulatorModel.Partitions.ElementAt(i));
                        return simulatorModel.Partitions.ElementAt(i);
                    }
                }
            }
            for (int i = 0; i < LastIndex; i++)
            {
                if (simulatorModel.Partitions.ElementAt(i).PartitionType == PartitionType.Free)
                {
                    if (simulatorModel.Partitions.ElementAt(i).Size >= allocation.RequiredSize)
                    {
                        LastGeneratedUserMemoryAddress = generateUserMemoryAddress(simulatorModel.Partitions.ElementAt(i));
                        return simulatorModel.Partitions.ElementAt(i);
                    }
                }
            }
            return null;
        }

        private int generateUserMemoryAddress(PartitionRecord partitionRecord)
        {
            int memoryShift = 0;
            foreach (PartitionRecord record in simulatorModel.Partitions)
            {
                memoryShift += record.Size;
                
                if (record.Equals(partitionRecord))
                {
                    return memoryShift;
                }
            } 
            return memoryShift;
        }

        private PartitionRecord getPartitionByMemoryAddress(int memoryAddress)
        {
            foreach (PartitionRecord record in simulatorModel.Partitions)
            { 
                if (memoryAddress - record.Size > 0)
                {
                    memoryAddress -= record.Size;
                }
                else
                {
                    return record;
                }
            }
            return simulatorModel.Partitions.Last();
        }
    }
}
