using Simulator.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAddressMapper.ViewModels;
using VirtualAddressMapper.Views;

namespace VirtualAddressMapper
{
    public class VirtualAddressMapper : Module
    {
        public override Type GetModuleViewModelType()
        {
            return (new VAM_ModuleViewModel()).GetType();
        }

        public override Type GetInputViewType()
        {
            return (new VAM_InputView()).GetType();
        }

        public override Type GetSimulationViewType()
        {
            return (new VAM_SimulationView()).GetType();
        }

        public override Type GetStatisticsViewType()
        {
            return (new VAM_StatisticsView()).GetType();
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
