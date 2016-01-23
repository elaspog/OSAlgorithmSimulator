using System;
using System.Xml.Serialization;

namespace VirtualAddressMapper.Models
{
    [Serializable]
    public class ResolveVirtualAddress : ActionBase
    {
        public ResolveVirtualAddress()
        {

        }

        public ResolveVirtualAddress(int _address)
        {
            address = _address;
        }

        int address;
        [XmlAttribute("Address")]
        public int Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }

    }
}
