using Simulator.Infrastructure.Repository;

namespace Simulator.Infrastructure.Services
{
    public interface IOBaseService
    {
        SimulationRecordWithModuleInfo ReadXmlHeader(string fileName);

        Simulator.Infrastructure.Module GetModuleInstanceBySimulationRecord(SimulationRecordWithModuleInfo simulationRecord);
    }
}
