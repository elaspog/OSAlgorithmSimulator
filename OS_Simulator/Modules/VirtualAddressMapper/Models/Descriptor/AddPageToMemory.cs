using System;
using System.Xml.Serialization;

namespace VirtualAddressMapper.Models
{
    [Serializable]
    public class AddPageToMemory : ActionMemoryBase
    {
        public AddPageToMemory()
        {

        }


        private int processPage;
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


    }
}
