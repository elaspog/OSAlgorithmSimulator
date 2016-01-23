using Simulator.Infrastructure;
using Simulator.Infrastructure.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MemoryAllocator.Models
{
    public class MA_SimulatorModel : Notifier, ISimulator
    {
        public MA_SimulatorModel()
        {
            metrics = new Metrics(this);
        }

        public void CreateSimulatorDomain(MA_Descriptor descriptor)
        {
            foreach (PartitionBase partition in descriptor.MemoryAllocation)
            {
                if (partition.GetType() == typeof(UsedPartition))
                {
                    Partitions.Add(new PartitionRecord(partition.Size, ((UsedPartition)partition).Id));
                }
                if (partition.GetType() == typeof(FreePartition))
                {
                    if (Partitions.Count != 0 && Partitions.Last() != null && Partitions.Last().PartitionType == PartitionType.Free)
                    {
                        Partitions.Last().mergeFreePartitions(partition.Size);
                    }
                    else
                    {
                        Partitions.Add(new PartitionRecord(partition.Size));
                    }
                }
            }
            AllocationRequestSequence = new ObservableCollection<AllocationActionBase>(descriptor.AllocationRequestSequence.Select(i => (AllocationActionBase)i.Clone()).ToList());

            LoadAllocatorAlgorithm(descriptor.AllocationAlgorithm);
        }

        private int previousAllocationRequestCount = 0;
        public bool NextStep()
        {

            if (AllocationRequestSequence.Count > 0 )
            {
                AllocationActionBase action = AllocationRequestSequence.ElementAt(0);
                if (action.GetType() == typeof(AllocateAction) && memoryAllocator != null)
                {
                    PartitionRecord partitionToUse = memoryAllocator.getPartition((AllocateAction)action);
                    if (partitionToUse != null)
                    {
                        AllocateSpaceOnPartition(partitionToUse, (AllocateAction)action);
                        AllocationRequestSequence.RemoveAt(0);
                    }
                }
                if (action.GetType() == typeof(FreeAction))
                {
                    PartitionRecord recordToUse = null;
                    bool flag = false;
                    foreach (PartitionRecord record in Partitions)
                    {
                        foreach (int recordId in record.Ids)
                        {
                            if (recordId == ((FreeAction)action).ExistingId)
                            {
                                recordToUse = record;
                                flag = true;
                                break;
                            }
                        }
                        if (flag)
                            break;
                    }
                    if (recordToUse != null)
                    {
                        int partitionIndex = Partitions.IndexOf(recordToUse);

                        PartitionRecord elementToRemove = Partitions.ElementAt(partitionIndex);
                        Partitions.RemoveAt(partitionIndex);
                        Partitions.Insert(partitionIndex, new PartitionRecord(elementToRemove.Size));
                        AllocationRequestSequence.RemoveAt(0);
                        mergeFreePartitions();
                    }
                }
            }
            metrics.Run();

            bool returnValue = true; ;
            if (previousAllocationRequestCount == AllocationRequestSequence.Count)
            {
                returnValue = false;
            }
            previousAllocationRequestCount = AllocationRequestSequence.Count;
            return returnValue;
        }

        private void mergeFreePartitions()
        {
            int PartitionsCount = Partitions.Count;

            if (PartitionsCount >= 2)
            {
                for (int i = 0; i < PartitionsCount; i++)
                { 
                    if ( i-1 >= 0 && 
                        Partitions.ElementAt(i).PartitionType == PartitionType.Free && 
                        Partitions.ElementAt(i-1).PartitionType == PartitionType.Free)
                    {
                        Partitions.ElementAt(i - 1).mergeFreePartitions(Partitions.ElementAt(i).Size);
                        Partitions.RemoveAt(i);
                        PartitionsCount--;
                    }
                    if (i + 1 <= PartitionsCount &&
                        Partitions.ElementAt(i).PartitionType == PartitionType.Free &&
                        Partitions.ElementAt(i + 1).PartitionType == PartitionType.Free)
                    {
                        Partitions.ElementAt(i).mergeFreePartitions(Partitions.ElementAt(i+1).Size);
                        Partitions.RemoveAt(i+1);
                        PartitionsCount--;
                    } 
                }
            }
        }

        private void AllocateSpaceOnPartition(PartitionRecord freePartitionToUse, AllocateAction allocateAction)
        {
            int partitionIndex = Partitions.IndexOf(freePartitionToUse);

            int difference = freePartitionToUse.Size - allocateAction.RequiredSize;
            Partitions.RemoveAt(partitionIndex);
            if (difference > 0)
            {
                Partitions.Insert(partitionIndex, new PartitionRecord(freePartitionToUse.Size - allocateAction.RequiredSize));
            }
            Partitions.Insert(partitionIndex, new PartitionRecord(allocateAction.RequiredSize, allocateAction.AllocatedNewSpaceId));
        }

        public bool PreviousStep()
        {
            throw new NotImplementedException();
        }

        private void LoadAllocatorAlgorithm(string AllocationAlgorithm)
        {
            switch (AllocationAlgorithm.ToUpper())
            {
                case "FIRSTFIT":
                case "FIRST FIT":
                case "FF":
                    memoryAllocator = new FirstFitAlgorithm(this);
                    break;
                case "NEXTFIT":
                case "NEXT FIT":
                case "NF":
                    memoryAllocator = new NextFitAlgorithm(this);
                    break;
                case "BESTFIT":
                case "BEST FIT":
                case "BF":
                    memoryAllocator = new BestFitAlgorithm(this);
                    break;
                case "WORSTFIT":
                case "WORST FIT":
                case "WF":
                    memoryAllocator = new WorstFitAlgorithm(this);
                    break;
                
            }
        }

        IAllocator memoryAllocator;
        Metrics metrics;
        public Metrics Metrics
        {
            get { return metrics; }
            set
            {
                metrics = value;
                OnPropertyChanged("Metrics");
            }
        }

        ObservableCollection<PartitionRecord> partitions = new ObservableCollection<PartitionRecord>();
        public ObservableCollection<PartitionRecord> Partitions
        {
            get { return partitions; }
            set
            {
                partitions = value;
                OnPropertyChanged("Partitions");
            }
        }

        private ObservableCollection<AllocationActionBase> allocationRequestSequence;
        public ObservableCollection<AllocationActionBase> AllocationRequestSequence
        {
            get { return allocationRequestSequence; }
            set
            {
                allocationRequestSequence = value;
                OnPropertyChanged("AllocationRequestSequence");
            }
        }
    }
}
