namespace Simulator.Infrastructure.Models
{
    public interface ISimulator
    {
        bool NextStep();

        bool PreviousStep();
        
    }
}
