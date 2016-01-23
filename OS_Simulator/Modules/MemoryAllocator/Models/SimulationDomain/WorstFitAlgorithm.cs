
namespace MemoryAllocator.Models
{
    public class WorstFitAlgorithm : IAllocator
    {
        public WorstFitAlgorithm(MA_SimulatorModel simulatorModel)
        {
            this.simulatorModel = simulatorModel;
        }

        MA_SimulatorModel simulatorModel;

        public PartitionRecord getPartition(AllocateAction allocation)
        {
            PartitionRecord recordToReturn = null;
            foreach (PartitionRecord record in simulatorModel.Partitions)
            {
                if (record.PartitionType == PartitionType.Free)
                {
                    if (record.Size >= allocation.RequiredSize)
                    {
                        if (recordToReturn != null)
                        {
                            if (recordToReturn.Size < record.Size)
                            {
                                recordToReturn = record;
                            }
                        }
                        else
                        {
                            recordToReturn = record;
                        }
                    }
                }
            }
            return recordToReturn;
        }
    }
}
