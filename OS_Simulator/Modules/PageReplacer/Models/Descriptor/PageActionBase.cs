using Simulator.Infrastructure;
using System.Xml.Serialization;

namespace PageReplacer.Models
{
    abstract public class PageActionBase : Notifier
    {
        public PageActionBase()
        {
             
        }

        private int page;
        [XmlAttribute("Page")]
        public int Page
        {
            get { return page; }
            set
            {
                page = value;
                OnPropertyChanged("Page");
            }
        }


        public PageActionBase Clone()
        {
            return (PageActionBase)this.MemberwiseClone();
        }
    }
}
