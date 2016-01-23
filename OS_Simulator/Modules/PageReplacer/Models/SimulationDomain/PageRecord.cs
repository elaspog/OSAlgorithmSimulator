using Simulator.Infrastructure;
using System;
using System.Collections.ObjectModel;

namespace PageReplacer.Models
{
    [Serializable]
    public class PageRecord : Notifier//, ICloneable
    {
        public PageRecord(int maxSize)
        {
            pages = new ObservableCollection<Page>();
            this.maxSize = maxSize;
        }

        private int maxSize;

        private ObservableCollection<Page> pages;
        public ObservableCollection<Page> Pages
        {
            get { return pages; }
            set
            {
                pages = value;
                OnPropertyChanged("Pages");
            }
        }

        private int ? referenced;
        public int ? Referenced
        {
            get { return referenced; }
            set
            {
                referenced = value;
                OnPropertyChanged("Referenced");
            }
        }

        private bool pageFault;
        public bool PageFault
        {
            get { return pageFault; }
            set
            {
                pageFault = value;
                OnPropertyChanged("PageFault");
            }
        }

        public PageRecord CreateNewPageByPageNumberAndShiftTheOtherPages (int id)
        {
            Page newPage = new Page(id);
            Pages.Insert(0, newPage);
            Referenced = id;
            PageFault = true;

            if (Pages.Count > maxSize)
            {
                for (int i = maxSize; i <= Pages.Count; i++)
                {
                    Pages.RemoveAt(i);
                }
            }
            return (PageRecord)this;
        }

        public PageRecord ReplaceExistingPageWithNewPage(Page oldPage, int newPageid)
        {
            Page newPage = new Page(newPageid);
            Referenced = newPageid;
            PageFault = true;

            int indexInPageList = -1;
            foreach (Page page in Pages)
            {
                if (oldPage.PageNumber == page.PageNumber)
                {
                    indexInPageList = Pages.IndexOf(page);
                }
            }

            if (indexInPageList >= 0)
            {
                if (Pages.Count + 1 > maxSize)
                {
                    Pages.RemoveAt(indexInPageList);
                }                
                Pages.Insert(indexInPageList, newPage);
            }
            return (PageRecord)this;

        }

        public void setReferenceAndPageFault(int i, bool p)
        {
            PageFault = p;
            Referenced = i;
        }

        public bool containsPage(int i)
        {
            foreach (Page page in Pages)
            {
                if (page.PageNumber == i)
                {
                    return true;
                }
            }
            return false;
        }

        public void setMbit(int i, bool v)
        {
            foreach (Page page in Pages)
            {
                if (page.PageNumber == i)
                {
                    page.Mbit = v;
                }
            }
        }

        public void setRbit(int i, bool v)
        {
            foreach (Page page in Pages)
            {
                if (page.PageNumber == i)
                {
                    page.Rbit = v;
                }
            }
        }

        public bool hasMoreSpaceForNewPage()
        {
            if (Pages.Count < maxSize)
                return true;
            else
                return false;
        }

        public void setTimestampOnPage(int pageNumber, int timestamp)
        {
            foreach (Page page in Pages)
            {
                if (page.PageNumber == pageNumber)
                {
                    page.LastUsedTimestamp = timestamp;
                }
            }
        }
    }
}
