using System.Collections.ObjectModel;
using System.Linq;


namespace TaskScheduler.Models
{
    class CareTaker
    {
        public CareTaker()
        { 
        }

        private ObservableCollection<Memento> mementos = new ObservableCollection<Memento>();
        public ObservableCollection<Memento> Mementos
        {
            get { return mementos; }
            set { mementos = value; }
        }

        public void AddMemento(Memento memento)
        {
            mementos.Add(memento);
        }

        public Memento GetMemento(int i)
        {
            return mementos.ElementAt(i);
        }
    }
}
