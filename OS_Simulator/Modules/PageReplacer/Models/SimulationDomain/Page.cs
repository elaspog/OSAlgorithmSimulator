using Simulator.Infrastructure;
using System;

namespace PageReplacer.Models
{
    [Serializable]
    public class Page : Notifier
    {
        public Page(int pageNumber)
        {
            this.PageNumber = pageNumber;
            Rbit = Mbit /*= Fbit*/ = false;
            LfuCounter = 0;
        }

        private int pageNumber;
        public int PageNumber
        {
            get { return pageNumber; }
            set
            {
                pageNumber = value;
                OnPropertyChanged("PageNumber");
            }
        }

        private bool rbit;
        public bool Rbit
        {
            get { return rbit; }
            set
            {
                rbit = value;
                OnPropertyChanged("Rbit");
            }
        }

        private bool mbit;
        public bool Mbit
        {
            get { return mbit; }
            set
            {
                mbit = value;
                OnPropertyChanged("Mbit");
            }
        }

        private int lastUsedTimestamp;
        public int LastUsedTimestamp
        {
            get { return lastUsedTimestamp; }
            set
            {
                lastUsedTimestamp = value;
                OnPropertyChanged("LastUsedTimestamp");
            }
        }

        private int? lfuCounter;
        public int? LfuCounter
        {
            get { return lfuCounter; }
            set
            {
                lfuCounter = value;
                OnPropertyChanged("LfuCounter");
            }
        }
        
    }
}
