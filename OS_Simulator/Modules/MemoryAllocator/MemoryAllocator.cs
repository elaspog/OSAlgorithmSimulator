using Simulator.Infrastructure;
using System;
using MemoryAllocator.Views;
using MemoryAllocator.ViewModels;

namespace MemoryAllocator
{
    public class MemoryAllocator : Module
    {
        public override Type GetModuleViewModelType()
        {
            return (new MA_ModuleViewModel()).GetType();
        }
        

        public override Type GetInputViewType()
        {
            return (new MA_InputView()).GetType();
        }

        public override Type GetSimulationViewType()
        {
            return (new MA_SimulationView()).GetType();
        }

        public override Type GetStatisticsViewType()
        {
            return (new MA_StatisticsView()).GetType();
        }

        public override Type GetDiagramVewType()
        {
            return null;
        }

        public override Type GetLogsViewType()
        {
            return null;
        }

        
    }
}
