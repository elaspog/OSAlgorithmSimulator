using Simulator.Infrastructure.Models;
using System;
using System.Collections.ObjectModel;

namespace TaskScheduler.Models
{
    class TS_PresenterModel : IPresenter
    {
        public TS_PresenterModel()
        {
            CareTaker = new ObservableCollection<Memento>();
        }

        ObservableCollection<Memento> CareTaker;
        

        public int GetActualCountOfStates
        {
            get { return CareTaker.Count; }
        }














        public void ShowState(int i)
        {
            throw new NotImplementedException();
        }



        int IPresenter.GetActualCountOfStates()
        {
            throw new NotImplementedException();
        }
    }
}
