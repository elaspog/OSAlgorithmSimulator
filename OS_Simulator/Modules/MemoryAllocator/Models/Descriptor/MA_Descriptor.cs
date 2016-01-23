using Simulator.Infrastructure.Models;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace MemoryAllocator.Models
{
    [XmlRootAttribute(ElementName = "MemoryAllocator")]
    public class MA_Descriptor : DescriptorBase
    {
        public MA_Descriptor()
        {
            Title = "Simulation";
            FQN_NamespaceAndClassName = "MemoryAllocator.MemoryAllocator";
            memoryAllocation = new ObservableCollection<PartitionBase>();
            allocationRequestSequence = new ObservableCollection<AllocationActionBase>();
        }

        private string allocationAlgorithm;
        [XmlElementAttribute(ElementName = "AllocationAlgorithm")]
        public string AllocationAlgorithm
        {
          get { return allocationAlgorithm; }
          set { 
                allocationAlgorithm = value;
                OnPropertyChanged("AllocationAlgorithm");
          }
        }

        private PhysicalMemory physicalMemory;
        [XmlElementAttribute(ElementName = "PhysicalMemory")]
        public PhysicalMemory PhysicalMemory
        {
            get { return physicalMemory; }
            set
            {
                physicalMemory = value;
                OnPropertyChanged("PhysicalMemory");
            }
        }

        private ObservableCollection<PartitionBase> memoryAllocation;
        [XmlArrayItem("FreePartition", Type = typeof(FreePartition))]
        [XmlArrayItem("UsedPartition", Type = typeof(UsedPartition))]
        public ObservableCollection<PartitionBase> MemoryAllocation
        {
            get 
            {
                ObservableCollection<PartitionBase> partitionsToContain = new ObservableCollection<PartitionBase>();
                int containedPartitionsSize = 0;
                foreach (PartitionBase partition in memoryAllocation)
                {
                    containedPartitionsSize += partition.Size;
                    if (containedPartitionsSize <= PhysicalMemory.UserMemorySize)
                    {
                        partitionsToContain.Add(partition);
                    }
                }
                memoryAllocation = partitionsToContain;
                return partitionsToContain;
            }
            set
            {
                ObservableCollection<PartitionBase> partitionsToContain = new ObservableCollection<PartitionBase>();
                int containedPartitionsSize = 0;
                foreach (PartitionBase partition in value)
                {
                    containedPartitionsSize += partition.Size;
                    if (containedPartitionsSize <= PhysicalMemory.UserMemorySize)
                    {
                        partitionsToContain.Add(partition);
                    }
                }
                memoryAllocation = partitionsToContain;
                OnPropertyChanged("MemoryAllocation");
            }
        }

        private ObservableCollection<AllocationActionBase> allocationRequestSequence;
        [XmlArrayItem("Free", Type = typeof(FreeAction))]
        [XmlArrayItem("Allocate", Type = typeof(AllocateAction))]
        public ObservableCollection<AllocationActionBase> AllocationRequestSequence
        {
            get { return allocationRequestSequence; }
            set
            {
                allocationRequestSequence = value;
                OnPropertyChanged("AllocationRequestSequence");
            }
        }

        [XmlIgnore]
        public int PartitionListSize
        {
            get
            {
                int partitionSize = 0;
                foreach (PartitionBase partition in MemoryAllocation)
                {
                    partitionSize += partition.Size;
                }
                return partitionSize;
            }
        }
    }
}
