using System;

namespace Simulator.Infrastructure
{
    public abstract class Module
    {
        public Module()
        {
        }

        // Modul inicializálásánál elengedhetetlenek
        abstract public Type GetInputViewType();
        abstract public Type GetSimulationViewType();
        abstract public Type GetStatisticsViewType();
        abstract public Type GetDiagramVewType();
        abstract public Type GetLogsViewType();

        //DataContext-nél elengedhetetlen
        abstract public Type GetModuleViewModelType();

    }
}
