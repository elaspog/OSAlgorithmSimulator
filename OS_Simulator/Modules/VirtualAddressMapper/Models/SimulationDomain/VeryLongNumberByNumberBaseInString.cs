using Simulator.Infrastructure;

namespace VirtualAddressMapper.Models
{
    public class VeryLongNumberByNumberBaseInString : Notifier
    {
        public VeryLongNumberByNumberBaseInString()
        {

        }

        public VeryLongNumberByNumberBaseInString(string number, int numberBase)
        {
            Number = number;
            NumberBase = numberBase;
        }

        private int numberBase;
        public int NumberBase
        {
            get { return numberBase; }
            set
            {
                numberBase = value;
                OnPropertyChanged("NumberBase");
            }
        }

        private string number;
        public string Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }

        public string getNumberInBase(int system)
        {
            return NumberRadixConverter.Convert(numberBase, system, number);
        }

        public void set(int system, string number)
        {
            Number = number;
            NumberBase = system;
        }


    }
}
