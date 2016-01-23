using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler.Models
{
    public class Memento
    {
        public Memento()
        { 
        }

        ResourceState resourceState;
        ProcessState processState;
        MetricsState metricsState;
        DiagramsState diagramsState;
        LogsState logsState;
    }
}
