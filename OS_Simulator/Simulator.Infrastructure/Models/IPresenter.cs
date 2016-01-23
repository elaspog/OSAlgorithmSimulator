namespace Simulator.Infrastructure.Models
{
    public interface IPresenter
    {
        int GetActualCountOfStates();

        void ShowState(int i);
    }
}
