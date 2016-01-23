using Simulator.Infrastructure;
using System.Collections.ObjectModel;

namespace PageReplacer.Models
{
    public class PageTable : Notifier
    {
        public PageTable()
        {
            pageRecords = new ObservableCollection<PageRecord>();
        }

        private ObservableCollection<PageRecord> pageRecords;
        public ObservableCollection<PageRecord> PageRecords
        {
            get { return pageRecords; }
            set
            {
                pageRecords = value;
                OnPropertyChanged("PageRecords");
            }
        }
    }
}
