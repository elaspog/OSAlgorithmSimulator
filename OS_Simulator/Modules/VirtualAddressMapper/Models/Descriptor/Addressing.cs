using Simulator.Infrastructure;
using System;
using System.Xml.Serialization;

namespace VirtualAddressMapper.Models
{
    [Serializable]
    public class Addressing : Notifier
    {
        public Addressing()
        {
            processAddressRangeByLastAddress = new ResolveVirtualAddress();
        }

        public Addressing(Addressing address)
        {
            
            this.AddressingBits = address.AddressingBits;
            this.BitsToAddressPages = address.BitsToAddressPages;
            this.MemoryAccessTime = address.MemoryAccessTime;
            this.QuantityOfPagesStorableInUserMemory = address.QuantityOfPagesStorableInUserMemory;
            this.IsAssociativeMemoryInUse = address.IsAssociativeMemoryInUse;
            this.QuantityOfAddressesStorableInAssociativeMemory = address.QuantityOfAddressesStorableInAssociativeMemory;
            this.AssociativeMemoryAccessTime = address.AssociativeMemoryAccessTime;
            this.ProcessAddressRangeByLastAddress = address.ProcessAddressRangeByLastAddress;
        }

        private int addressingBits;
        [XmlAttribute("AddressingBits")]
        public int AddressingBits
        {
            get { return addressingBits; }
            set
            {
                if ((int)value == 8 || (int)value == 16 || (int)value == 32 || (int)value == 64 || (int)value == 128)
                {
                    addressingBits = value;
                }
                OnPropertyChanged("AddressingBits");
            }
        }

        private int bitsToAddressPages;
        [XmlElementAttribute(ElementName = "BitsToAddressPages")]
        public int BitsToAddressPages
        {
            get { return bitsToAddressPages; }
            set
            {
                if ((int)value < addressingBits)
                {
                    bitsToAddressPages = value;
                    bitsToAddressOnPage = addressingBits - bitsToAddressPages;
                }
                OnPropertyChanged("BitsToAddressPages");
                OnPropertyChanged("BitsToAddressOnPage");
            }
        }

        [XmlIgnore]
        private int bitsToAddressOnPage;
        public int BitsToAddressOnPage
        {
            get { return bitsToAddressOnPage; }
        }


        private int memoryAccessTime;
        [XmlElementAttribute(ElementName = "MemoryAccessTime")]
        public int MemoryAccessTime
        {
            get { return memoryAccessTime; }
            set
            {
                memoryAccessTime = value;
                OnPropertyChanged("MemoryAccessTime");
            }
        }


        private int quantityOfPagesStorableInUserMemory;
        [XmlElementAttribute(ElementName = "QuantityOfPagesStorableInUserMemory")]
        public int QuantityOfPagesStorableInUserMemory
        {
            get { return quantityOfPagesStorableInUserMemory; }
            set
            {
                quantityOfPagesStorableInUserMemory = value;
                OnPropertyChanged("QuantityOfPagesStorableInUserMemory");
            }
        }


        private bool isAssociativeMemoryInUse;
        [XmlElementAttribute(ElementName = "IsAssociativeMemoryInUse")]
        public bool IsAssociativeMemoryInUse
        {
            get { return isAssociativeMemoryInUse; }
            set
            {
                isAssociativeMemoryInUse = value;
                OnPropertyChanged("IsAssociativeMemoryInUse");
            }
        }


        private int quantityOfAddressesStorableInAssociativeMemory;
        [XmlElementAttribute(ElementName = "QuantityOfAddressesStorableInAssociativeMemory")]
        public int QuantityOfAddressesStorableInAssociativeMemory
        {
            get { return quantityOfAddressesStorableInAssociativeMemory; }
            set
            {
                quantityOfAddressesStorableInAssociativeMemory = value;
                OnPropertyChanged("QuantityOfAddressesStorableInAssociativeMemory");
            }
        }


        private int associativeMemoryAccessTime;
        [XmlElementAttribute(ElementName = "AssociativeMemoryAccessTime")]
        public int AssociativeMemoryAccessTime
        {
            get { return associativeMemoryAccessTime; }
            set
            {
                associativeMemoryAccessTime = value;
                OnPropertyChanged("AssociativeMemoryAccessTime");
            }
        }

        private ResolveVirtualAddress processAddressRangeByLastAddress;
        [XmlElementAttribute(ElementName = "ProcessAddressRangeByLastAddress")]
        public ResolveVirtualAddress ProcessAddressRangeByLastAddress
        {
            get { return processAddressRangeByLastAddress; }
            set
            {
                processAddressRangeByLastAddress = value;
                OnPropertyChanged("ProcessAddressRangeByLastAddress");
            }
        }
    }
}
