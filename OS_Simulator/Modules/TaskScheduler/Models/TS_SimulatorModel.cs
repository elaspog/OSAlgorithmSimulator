using Simulator.Infrastructure;
using Simulator.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TaskScheduler.Models
{
    public class TS_SimulatorModel : Notifier, ISimulator
    {
        public TS_SimulatorModel()
        {
            ActualStep = 0;

            initialProcessList = new ObservableCollection<Process>();
            arrivedProcesses = new ObservableCollection<Process>();
            resources = new ObservableCollection<Resource>();

            queue = new ObservableCollection<Process>();
            GanntHistory = new GanntHistory();
           
        }


        int actualStep;

        public int ActualStep
        {
            get { return actualStep; }
            set
            {
                actualStep = value;
                OnPropertyChanged("ActualStep");
            }
        }

        ObservableCollection<Process> initialProcessList;
        ObservableCollection<Process> arrivedProcesses;
        ObservableCollection<Resource> resources;
        ITaskScheduler taskScheduler;
        private Metrics metrics;

        public ObservableCollection<Process> InitialProcessList
        {
            get { return initialProcessList; }
            set
            {
                initialProcessList = value;
                OnPropertyChanged("InitialProcessList");
            }
        }
        public ObservableCollection<Process> ArrivedProcesses
        {
            get { return arrivedProcesses; }
            set
            {
                arrivedProcesses = value;
                OnPropertyChanged("ArrivedProcesses");
            }
        }
        public ObservableCollection<Resource> Resources
        {
            get { return resources; }
            set
            {
                resources = value;
                OnPropertyChanged("Resources");
            }
        }
        public Metrics Metrics
        {
            get { return metrics; }
            set
            {
                metrics = value;
                OnPropertyChanged("Metrics");
            }
        }
        Process prevRunningProcess = null;

        private GanntHistory ganntHistory;
        public GanntHistory GanntHistory
        {
            get { return ganntHistory; }
            set
            {
                ganntHistory = value;
                OnPropertyChanged("GanntHistory"); 
            }
        }
        ObservableCollection<Process> queue;
        public ObservableCollection<Process> Queue
        {
            get { return queue; }
            set
            {
                queue = value;
                OnPropertyChanged("Queue");
            }
        }



        public void CreateSimulatorDomain(TS_Descriptor descriptor)
        {
            // erőforrások létrehozása
            foreach (ResourceDescriptor resourceDescriptor in descriptor.Resources)
            {
                Resource resource = new Resource(resourceDescriptor, this);
                Resources.Add(resource);
            }
            // processzek létrehozása és használt erőforrásainak ellenőrzése
            foreach (ProcessDescriptor processDescriptor in descriptor.Processes)
            {
                Process process = new Process(this, MyCloner.DeepClone<ProcessDescriptor>(processDescriptor), Resources);
                InitialProcessList.Add(process);
            }

            Metrics = new Metrics(this, descriptor);

            LoadSchedulerAlgorithm(descriptor.SchedulerAlgorithm);

            Process runningProcess = ArrivedProcesses.FirstOrDefault(x => x.ProcessStatus == ProcessStatusEnum.Running);
            GanntHistory.AddToHistory(ActualStep, runningProcess, queue);

            InitiateProcesses();
            RunSchedulerIfNecassary();
        }

        private void LoadSchedulerAlgorithm(SchedulerAlgorithmDescriptor schedulerAlgorithmDescriptor)
        {
            switch (schedulerAlgorithmDescriptor.AlgorithmName)
            {
                case "FIFO":
                case "FCFS":
                    taskScheduler = new FIFO(this, schedulerAlgorithmDescriptor.Parameters);
                    break;
                case "RR":
                    taskScheduler = new RR(this, schedulerAlgorithmDescriptor.Parameters);
                    break;
                case "SJF":
                    taskScheduler = new SJF(this, schedulerAlgorithmDescriptor.Parameters);
                    break;
                case "SRTF":
                    taskScheduler = new SRTF(this, schedulerAlgorithmDescriptor.Parameters);
                    break;
            }
        }

        private void RunSchedulerIfNecassary()
        {
            Process runningProcess = ArrivedProcesses.FirstOrDefault(x => x.ProcessStatus == ProcessStatusEnum.Running);

            switch (taskScheduler.GetActingAndPreemptivityType())
            {
                case AlgorithmActingAndPreemptivityType.None:

                    if (runningProcess == null)
                    {
                        checkNullProcesses();
                        RunScheduler();
                    }

                    break;
                case AlgorithmActingAndPreemptivityType.ByTimeSlice:

                    ((ITaskSchedulerByTime)taskScheduler).updateTime(ActualStep);

                    if (runningProcess == null || ((ITaskSchedulerByTime)taskScheduler).needToRun() == true)
                    {
                        checkNullProcesses();
                        RunScheduler();
                    }

                    break;
                case AlgorithmActingAndPreemptivityType.ByArrivedProcess:
                    
                    if (runningProcess == null || ((ITaskSchedulerByEvent)taskScheduler).needToRun() == true)
                    {
                        checkNullProcesses();
                        RunScheduler();
                    }

                    ((ITaskSchedulerByEvent)taskScheduler).update();

                    break;
            }
        }

        private void checkNullProcesses()
        {
            foreach (Process process in ArrivedProcesses)
            { 
                if (process.BurstSequence.Count <= 0)
                {
                    process.ProcessStatus = ProcessStatusEnum.Finished;
                }

                if (process.CpuTimeLeft <= 0)
                { 
                    if (Queue.Contains(process))
                    {
                        Queue.Remove(process);
                    }
                }
            }
        }

        bool schedulerWasRunning = false;
        private void RunScheduler()
        {
            Process nextProcessToRun = taskScheduler.get();
            if (nextProcessToRun != null)
            {
                nextProcessToRun.ProcessStatus = ProcessStatusEnum.Running;
                schedulerWasRunning = true;
            }
        }

        private void ElpaseTime()
        {
            foreach (Resource resource in Resources)
            {
                resource.ElpaseResourceTime();
            }

            foreach (Process process in ArrivedProcesses)
            {
                if (process.ProcessStatus != ProcessStatusEnum.Running)
                {
                    process.Wait();
                }
            }
            Process runningProcess = ArrivedProcesses.FirstOrDefault(x => x.ProcessStatus == ProcessStatusEnum.Running);
            if (runningProcess != null)
            {
                runningProcess.ElpaseCpuTime();
                Metrics.CountOfMillisecondsWhereProcessWasRunning++;
            }
            else
            {
                Metrics.CountOfMillisecondsWhereProcessWasNotRunning++;
            }

            if (prevRunningProcess != runningProcess)
            {
                if (runningProcess != null)
                {
                    Metrics.CountOfContextSwitchings++;
                }
            }
            prevRunningProcess = runningProcess;
        }


        public bool NextStep()
        {
            ActualStep++;

            Process runningProcess = ArrivedProcesses.FirstOrDefault(x => x.ProcessStatus == ProcessStatusEnum.Running);
            GanntHistory.AddToHistory(ActualStep, runningProcess, queue);

            ElpaseTime();

            if (schedulerWasRunning)
            {
                Metrics.CountOfTaskSchedulings++;
                schedulerWasRunning = false;
            }
            Metrics.run();
            Metrics.update();

            InitiateProcesses();
            RunSchedulerIfNecassary();


            int allProcessCount = 0;
            allProcessCount += InitialProcessList.Count;
            allProcessCount += arrivedProcesses.Count;

            int finishedProcessCount = ArrivedProcesses.Where(x => x.ProcessStatus == ProcessStatusEnum.Finished).Count();

            if (allProcessCount == finishedProcessCount)
            {
                return false ;
            }
            else
            {
                return true;
            }

            
        }

        private void InitiateProcesses()
        {
            if (InitialProcessList != null && InitialProcessList.Count > 0)
            {
                ObservableCollection<Process> processesToInitiate = new ObservableCollection<Process>();

                foreach (Process process in InitialProcessList)
                {
                    if (process.ArrivalTime == ActualStep)
                    {
                        processesToInitiate.Add(process);
                        process.ProcessStatus = ProcessStatusEnum.Ready;
                        Queue.Add(process);
                    }
                }
                foreach (Process process in processesToInitiate)
                {
                    ArrivedProcesses.Add(process);
                    InitialProcessList.Remove(process);
                }
            }
        }

        public bool PreviousStep()
        {
            throw new NotImplementedException();
        }

        // Process hívhatja, e formában kéri az operációs rendszertől az erőforrások lefoglalását
        public void UseResources(Process process, List<IoBurstDescriptor> initResourcesList)
        {
            foreach (IoBurstDescriptor ioBurst in initResourcesList)
            {
                Resource executingResource = null;
                foreach (Resource resource in Resources)
                {
                    if (resource.ResourceName.Equals(ioBurst.ResourceName))
                    {
                        executingResource = resource;
                        break;
                    }
                }
                if (executingResource != null)
                {
                    // ellenőrizni kell, hogy nem adtuk-e már korábban hozzá az erőforráshoz ugyanezeket a burstöket
                    bool wasAddedToResource = false;
                    foreach (ResourceRecord record in executingResource.CurrentlyUsing.Concat(executingResource.Queue))
                    {
                        if (record.Process.ProcessName.Equals(process.ProcessName)
                            && record.Burst.Equals(ioBurst))
                        {
                            wasAddedToResource = true;
                        }
                    }
                    if (!wasAddedToResource)
                    {
                        executingResource.addProcess(new ResourceRecord(process, ioBurst));
                    }
                }
            }
        }


    }
}
