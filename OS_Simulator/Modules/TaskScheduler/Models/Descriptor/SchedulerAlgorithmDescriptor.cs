using Simulator.Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace TaskScheduler.Models
{
    [Serializable]
    public class SchedulerAlgorithmDescriptor : Notifier
    {
        public SchedulerAlgorithmDescriptor()
        {
        }

        public SchedulerAlgorithmDescriptor(string _algorithmName)
        {
            algorithmName = _algorithmName;
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
