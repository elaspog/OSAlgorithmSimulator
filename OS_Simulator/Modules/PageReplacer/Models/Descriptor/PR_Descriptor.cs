using Simulator.Infrastructure.Models;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace PageReplacer.Models
{
    [XmlRootAttribute(ElementName = "PageReplacer")]
    public class PR_Descriptor : DescriptorBase
    {
        public PR_Descriptor()
        {
            Title = "Simulation";
            FQN_NamespaceAndClassName = "PageReplacer.PageReplacer";
            pageActionSequence = new ObservableCollection<PageActionBase>();
        }

        private int countOfPagesForProcess;
        [XmlElementAttribute(ElementName = "CountOfPagesForProcess")]
        public int CountOfPagesForProcess
        {
            get { return countOfPagesForProcess; }
            set
            {
                countOfPagesForProcess = value;
                OnPropertyChanged("CountOfPagesForProcess");
            }
        }

        private PageReplacerAlgorithm pageReplacerAlgorithm;
        [XmlElementAttribute(ElementName = "PageReplacerAlgorithm")]
        public PageReplacerAlgorithm PageReplacerAlgorithm
        {
            get { return pageReplacerAlgorithm; }
            set
            {
                pageReplacerAlgorithm = value;
                OnPropertyChanged("PageReplacerAlgorithm");
            }
        }


        private ObservableCollection<PageActionBase> pageActionSequence;
        [XmlArrayItem("AccessPage", Type = typeof(PageAccess))]
        [XmlArrayItem("SetRbitOnPage", Type = typeof(PageSetRbit))]
        [XmlArrayItem("SetMbitOnPage", Type = typeof(PageSetMbit))]
        [XmlArrayItem("RemoveRbitOnPage", Type = typeof(PageRemoveRbit))]
        [XmlArrayItem("RemoveMbitOnPage", Type = typeof(PageRemoveMbit))]
        [XmlArrayItem("PeriodRemoveAllRbit", Type = typeof(PeriodRemoveAllRbit))]
        public ObservableCollection<PageActionBase> PageActionSequence
        {
            get { return pageActionSequence; }
            set
            {
                pageActionSequence = value;
                OnPropertyChanged("PageActionSequence");
            }
        }


        private int memoryAccessTime;
        [XmlElementAttribute(ElementName = "MemoryAccessTime")]
        public int MemoryAccessTime
        {
            get { return memoryAccessTime; }
            set
            {
                memoryAccessTime = value;
                OnPropertyChanged("MemoryAccessTime");
            }
        }


        private int pageFaultServiceTime;
        [XmlElementAttribute(ElementName = "PageFaultServiceTime")]
        public int PageFaultServiceTime
        {
            get { return pageFaultServiceTime; }
            set
            {
                pageFaultServiceTime = value;
                OnPropertyChanged("PageFaultServiceTime");
            }
        }
    }
}
