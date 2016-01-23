using System;


namespace TaskScheduler.Models
{
    class Originator
    {
        public Originator()
        { 
        }

        private ResourceState resourceState;
        private ProcessState processState;
        private MetricsState metricsState;
        private DiagramsState diagramsState;
        private LogsState logsState;

        public ResourceState ResourceState
        {
            get { return resourceState; }
            set { resourceState = value; }
        }
        public ProcessState ProcessState
        {
            get { return processState; }
            set { processState = value; }
        }
        public MetricsState MetricsState
        {
            get { return metricsState; }
            set { metricsState = value; }
        }
        public DiagramsState DiagramsState
        {
            get { return diagramsState; }
            set { diagramsState = value; }
        }
        public LogsState LogsState
        {
            get { return logsState; }
            set { logsState = value; }
        }

        public Memento CreateMemento()
        {
            throw new NotImplementedException();
        }

        public void SetMemento(Memento m)
        {
            throw new NotImplementedException();
        }

    }
}
