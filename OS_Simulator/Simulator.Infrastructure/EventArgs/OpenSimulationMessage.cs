using Simulator.Infrastructure.Repository;

namespace Simulator.Infrastructure.EventArgs
{
    public class OpenSimulationMessage : WindowMessageBase
    {
        public string stringParam;

        public OpenSimulationMessage(SimulationRecordWithModuleInfo simulationRecord, string stringParam)
            : base(simulationRecord)
        {
            this.stringParam = stringParam;
        }
    }
}
