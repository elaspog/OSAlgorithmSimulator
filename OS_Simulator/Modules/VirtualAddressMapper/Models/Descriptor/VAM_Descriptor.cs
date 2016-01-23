using Simulator.Infrastructure.Models;
using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace VirtualAddressMapper.Models
{
    [Serializable]
    [XmlRootAttribute(ElementName = "VirtualAddressMapper")]
    public class VAM_Descriptor : DescriptorBase
    {
        public VAM_Descriptor()
        {
            Title = "Simulation";
            FQN_NamespaceAndClassName = "VirtualAddressMapper.VirtualAddressMapper";

            addressing = new Addressing();
            processPageAndMemoryPageMapping = new ObservableCollection<MappingRecordOfProcessAndMemory>();
            actionSequence = new ObservableCollection<ActionBase>();
            associativeMemoryInitialContent = new ObservableCollection<int>();
        }



        private Addressing addressing;
        [XmlElementAttribute(ElementName = "Addressing")]
        public Addressing Addressing
        {
            get { return addressing; }
            set
            {
                addressing = value;
                OnPropertyChanged("Addressing");
            }
        }


        private ObservableCollection<MappingRecordOfProcessAndMemory> processPageAndMemoryPageMapping;
        [XmlArrayItem("Mapping", Type = typeof(MappingRecordOfProcessAndMemory))]
        public ObservableCollection<MappingRecordOfProcessAndMemory> ProcessPageAndMemoryPageMapping
        {
            get { return processPageAndMemoryPageMapping; }
            set
            {
                processPageAndMemoryPageMapping = value;
                OnPropertyChanged("ProcessPageAndMemoryPageMapping");
            }
        }

        private ObservableCollection<int> associativeMemoryInitialContent;
        [XmlArrayItem("Page", typeof(int))]
        public ObservableCollection<int> AssociativeMemoryInitialContent
        {
            get { return associativeMemoryInitialContent; }
            set
            {
                associativeMemoryInitialContent = value;
                OnPropertyChanged("AssociativeMemoryInitialContent");
            }
        }


        private ObservableCollection<ActionBase> actionSequence;
        [XmlArrayItem("ResolveVirtualAddress", typeof(ResolveVirtualAddress))]
        [XmlArrayItem("AddToAssociativeMemory", typeof(AddPageToAssociativeMemory))]
        [XmlArrayItem("RemoveFromAssociativeMemory", typeof(RemovePageFromAssociativeMemory))]
        [XmlArrayItem("AddPageToMemory", typeof(AddPageToMemory))]
        [XmlArrayItem("RemovePageFromMemory", typeof(RemovePageFromMemory))]
        public ObservableCollection<ActionBase> ActionSequence
        {
            get { return actionSequence; }
            set
            {
                actionSequence = value;
                OnPropertyChanged("ReferencedVirtualAddressSequence");
            }
        }

    }
}
