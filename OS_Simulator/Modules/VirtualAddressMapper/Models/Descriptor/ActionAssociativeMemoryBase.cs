using System;
using System.Xml.Serialization;

namespace VirtualAddressMapper.Models
{
    [Serializable]
    public abstract class ActionAssociativeMemoryBase : ActionBase
    {
        public ActionAssociativeMemoryBase()
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
