using Simulator.Infrastructure;
using System;
using TaskScheduler.ViewModels;
using TaskScheduler.Views;

namespace TaskScheduler
{
    public class TaskScheduler : Module
    {
        public TaskScheduler() 
            : base()
        { 
             
        }
        
        public override Type GetModuleViewModelType()
        {
            return (new TS_ModuleViewModel()).GetType();
        }

        
        public override Type GetInputViewType()
        {
            return (new TS_InputView()).GetType();
        }

        public override Type GetSimulationViewType()
        {
            return (new TS_SimulationView()).GetType();
        }

        public override Type GetStatisticsViewType()
        {
            return (new TS_StatisticsView()).GetType();
        }

        public override Type GetDiagramVewType()
        {
            return (new TS_DiagramsView()).GetType();
        }

        public override Type GetLogsViewType()
        {
            return (new TS_LogsView()).GetType();
        }
        
        
    }
}
