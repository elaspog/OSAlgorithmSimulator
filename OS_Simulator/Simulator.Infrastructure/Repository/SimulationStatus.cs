namespace Simulator.Infrastructure.Repository
{   
    // Initial = kiinduló
    // Corrupted = nem illeszhető a Descriptorra
    // Inconsistent = nem képezhető belőle Simulator model
    // Runnable = futtatásra kész
    // Finished = már nem futtatható tovább
    public enum SimulationStatus
    { 
        Initial, Corrupted, Inconsistent, Runnable, Finished 
    };
}
