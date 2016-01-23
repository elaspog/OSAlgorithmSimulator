using Simulator.Infrastructure.Models;
using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace TaskScheduler.Models
{
    [Serializable]
    [XmlRootAttribute(ElementName = "TaskScheduler")]
    public class TS_Descriptor : DescriptorBase
    {
        public TS_Descriptor()
        {
            resources = new ObservableCollection<ResourceDescriptor>();
            processes = new ObservableCollection<ProcessDescriptor>();
            Title = "Simulation";
            FQN_NamespaceAndClassName = "TaskScheduler.TaskScheduler";
        }

        private SchedulerAlgorithmDescriptor schedulerAlgorithm;
        [XmlElementAttribute(ElementName = "SchedulerAlgorithm")]
        public SchedulerAlgorithmDescriptor SchedulerAlgorithm
        {
            get { return schedulerAlgorithm; }
            set
            {
                schedulerAlgorithm = value;
                OnPropertyChanged("SchedulerAlgorithm");
            }
        }

        private ObservableCollection<ResourceDescriptor> resources;
        [XmlArrayItem("Resource")]
        public ObservableCollection<ResourceDescriptor> Resources
        {
            get { return resources; }
            set
            {
                resources = value;
                OnPropertyChanged("Resources");
            }
        }

        private ObservableCollection<ProcessDescriptor> processes;
        [XmlArrayItem("Process")]
        public ObservableCollection<ProcessDescriptor> Processes
        {
            get { return processes; }
            set
            {
                processes = value;
                OnPropertyChanged("Processes");
            }
        }

        private int avarageOverheadTime;
        [XmlElementAttribute(ElementName = "AvarageOverheadTime")]
        public int AvarageOverheadTime
        {
            get { return avarageOverheadTime; }
            set
            {
                avarageOverheadTime = value;
                OnPropertyChanged("AvarageOverheadTime");
            }
        }

    }
}
