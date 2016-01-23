using Simulator.Infrastructure.Repository;

namespace Simulator.Infrastructure.Models
{
    public interface IConfigurator
    {
        SimulationStatus InicializeModuleByStream(System.IO.Stream XMLInputstream);

    }
}
