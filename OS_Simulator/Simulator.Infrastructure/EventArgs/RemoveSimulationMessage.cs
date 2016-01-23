using Simulator.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator.Infrastructure.EventArgs
{
    public class RemoveSimulationMessage : WindowMessageBase
    {
        public string stringParam;

        public RemoveSimulationMessage(SimulationRecordWithModuleInfo simulationRecord, string stringParam)
            : base(simulationRecord)
        {
            this.stringParam = stringParam;
        }
    }
}
