namespace MemoryAllocator.Models
{
    interface IAllocator
    {
        PartitionRecord getPartition(AllocateAction allocation);
    }
}
