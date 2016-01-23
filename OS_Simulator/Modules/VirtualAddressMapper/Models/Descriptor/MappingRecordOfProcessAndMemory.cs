using Simulator.Infrastructure;
using System;
using System.Xml.Serialization;

namespace VirtualAddressMapper.Models
{
    [Serializable]
    public class MappingRecordOfProcessAndMemory : Notifier
    {
        public MappingRecordOfProcessAndMemory()
        {

        }

        public MappingRecordOfProcessAndMemory(int _processPage, int _memoryFramePageNumber)
        {
            processPage = _processPage;
            memoryFrame = _memoryFramePageNumber;
        }

        int processPage;
        [XmlAttribute("ProcessPage")]
        public int ProcessPage
        {
            get { return processPage; }
            set
            {
                processPage = value;
                OnPropertyChanged("ProcessPage");
            }
        }

        int memoryFrame;
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
