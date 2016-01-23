using System.Xml.Serialization;

namespace Simulator.Infrastructure.Models
{
    abstract public class DescriptorBase : Notifier
    {
        public DescriptorBase()
        {
        }


        protected string fqn_NamespaceAndClassName;
        [XmlAttribute("FQN_NamespaceAndClassName")]
        public string FQN_NamespaceAndClassName
        {
            get { return fqn_NamespaceAndClassName; }
            set
            {
                fqn_NamespaceAndClassName = value;
                OnPropertyChanged("FQN_NamespaceAndClassName");
            }
        }

        protected string title;
        [XmlAttribute("Title")]
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }
    }
}
