
namespace MemoryAllocator.Models
{
    public class FirstFitAlgorithm : IAllocator
    {
        public FirstFitAlgorithm(MA_SimulatorModel simulatorModel)
        {
            this.simulatorModel = simulatorModel;
        }

        MA_SimulatorModel simulatorModel;


        public PartitionRecord getPartition(AllocateAction allocation)
        {
            foreach (PartitionRecord record in simulatorModel.Partitions)
            {
                if (record.PartitionType == PartitionType.Free)
                {
                    if (record.Size >= allocation.RequiredSize)
                    {
                        return record;
                    }
                }
            }
            return null;
        }
    }
}
