using Simulator.Infrastructure;

namespace VirtualAddressMapper.Models
{
    public class IndicatorRecord : Notifier
    {
        public IndicatorRecord(int? _page, bool _flag)
        {
            page = _page;
            flag = _flag;
        }


        public IndicatorRecord(int? _page, int? _frame, bool _flag)
        {
            page = _page;
            flag = _flag;
            frame = _frame;
        }

        private int? frame;
        public int? Frame
        {
            get { return frame; }
            set
            {
                frame = value;
                OnPropertyChanged("Frame");
            }
        }


        private int? page;
        public int? Page
        {
            get { return page; }
            set
            {
                page = value;
                OnPropertyChanged("Page");
            }
        }


        private bool flag;
        public bool Flag
        {
            get { return flag; }
            set
            {
                flag = value;
                OnPropertyChanged("Flag");
            }
        }



    }
}
