using Simulator.Infrastructure;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace PageReplacer.Models
{
    public class PageReplacerAlgorithm : Notifier
    {
        public PageReplacerAlgorithm()
        {
            parameters = new ObservableCollection<string>();
        }

        private string algorithmName;
        [XmlElementAttribute(ElementName = "AlgorithmName")]
        public string AlgorithmName
        {
            get { return algorithmName; }
            set
            {
                algorithmName = value;
                OnPropertyChanged("AlgorithmName");
            }
        }

        private ObservableCollection<string> parameters;
        [XmlArrayItem("Parameter")]
        public ObservableCollection<string> Parameters
        {
            get { return parameters; }
            set
            {
                parameters = value;
                OnPropertyChanged("Parameters");
            }
        }


    }
}
