using System;
using System.Xml.Serialization;

namespace VirtualAddressMapper.Models
{
    [Serializable]
    public class ActionMemoryBase : ActionBase
    {
        public ActionMemoryBase()
        {

        }


        private int memoryFrame;
        [XmlAttribute("MemoryFrame")]
        public int MemoryFrame
        {
            get { return memoryFrame; }
            set
            {
                memoryFrame = value;
                OnPropertyChanged("MemoryFrame");
            }
        }


    }
}
