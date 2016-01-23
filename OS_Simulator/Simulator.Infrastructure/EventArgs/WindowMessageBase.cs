using Simulator.Infrastructure.Repository;

namespace Simulator.Infrastructure.EventArgs
{
    public class WindowMessageBase
    {
        public WindowMessageBase(SimulationRecordWithModuleInfo simulationRecord)
        {
            SimulationRecord = simulationRecord;
        }

        SimulationRecordWithModuleInfo simulationRecord;
        public SimulationRecordWithModuleInfo SimulationRecord
        {
            get { return simulationRecord; }
            set { simulationRecord = value; }
        }
    }
}
