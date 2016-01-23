using PageReplacer.ViewModels;
using PageReplacer.Views;
using Simulator.Infrastructure;
using System;

namespace PageReplacer
{
    public class PageReplacer : Module
    {
        public override Type GetModuleViewModelType()
        {
            return (new PR_ModuleViewModel()).GetType();
        }

        public override Type GetInputViewType()
        {
            return (new PR_InputView()).GetType();
        }

        public override Type GetSimulationViewType()
        {
            return (new PR_SimulationView()).GetType();
        }

        public override Type GetStatisticsViewType()
        {
            return (new PR_StatisticsView()).GetType();
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
