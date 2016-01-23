using Simulator.Infrastructure;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TaskScheduler.Models
{
    public enum ProcessStatusEnum
    {
        Undefined, Running, Ready, Waiting, Finished
    }

    public class Process : Notifier
    {
        public Process(TS_SimulatorModel simulatorModel, ProcessDescriptor processDescriptor, ObservableCollection<Resource> Resources) 
        {
            this.simulatorModel = simulatorModel;

            //ProcessStatus = ProcessStatusEnum.Undefined;
            ProcessName = processDescriptor.ProcessName;
            ArrivalTime = processDescriptor.ArrivalTime;

            // Burst-ök ellenőrése, amelyiknél nincs megfelelő erőforrás definiálva, törlésre kerül.
            BurstSequence = new ObservableCollection<BurstDescriptor>(processDescriptor.BurstSequence);
            List<BurstDescriptor> burstsWithIllegalResourceList = new List<BurstDescriptor>();
            foreach (BurstDescriptor burst in BurstSequence)
            {
                if (burst.BurstTime <= 0)
                {
                    burstsWithIllegalResourceList.Add(burst);
                    break;
                }

                if (burst.GetType() == typeof(IoBurstAsynchronousDescriptor)
                    || burst.GetType() == typeof(IoBurstSynchronousDescriptor))
                {
                    IoBurstDescriptor burstToCheckResource = (IoBurstDescriptor)burst;
                    
                    bool isResourceDefined = false;
                    foreach (Resource resource in Resources)
                    { 
                        if (resource.ResourceName.Equals(burstToCheckResource.ResourceName))
                        {
                            isResourceDefined = true;
                            break;
                        }
                    }
                    if (! isResourceDefined)
                    {
                        burstsWithIllegalResourceList.Add(burst);
                    }
                }
            }
            foreach (BurstDescriptor illegalBurst in burstsWithIllegalResourceList)
            {
                BurstSequence.Remove(illegalBurst);
            }

            // alapértékek beállítása az esetlegesen módosított BurstSequence alapján
            cpuTimeLeft = asynchronousTimeRunned = synchronousTimeRunned = 0;
            cpuTimeLeft = asynchronousTimeLeft = synchronousTimeLeft = 0;
            foreach (CpuBurstDescriptor burst in BurstSequence.OfType<CpuBurstDescriptor>())
            {
                cpuTimeLeft += burst.BurstTime;
            }
            foreach (IoBurstAsynchronousDescriptor burst in BurstSequence.OfType<IoBurstAsynchronousDescriptor>())
            {
                asynchronousTimeLeft += burst.BurstTime;
            }
            foreach (IoBurstSynchronousDescriptor burst in BurstSequence.OfType<IoBurstSynchronousDescriptor>())
            {
                synchronousTimeLeft += burst.BurstTime;
            }
            
        }

        private TS_SimulatorModel simulatorModel;

        private string processName;
        private ProcessStatusEnum processStatus;
        private int arrivalTime;
        private int cpuTimeRunned;
        private int asynchronousTimeRunned;
        private int synchronousTimeRunned;
        private int timeWaited;
        private int cpuTimeLeft;
        private int asynchronousTimeLeft;
        private int synchronousTimeLeft;
        private ObservableCollection<BurstDescriptor> burstSequence;



        public string ProcessName
        {
            get { return processName; }
            set
            {
                processName = value;
                OnPropertyChanged("ProcessName");
            }
        }
        public ProcessStatusEnum ProcessStatus
        {
            get { return processStatus; }
            set
            {
                processStatus = value;
                OnPropertyChanged("ProcessStatus");
            }
        }
        public int ArrivalTime
        {
            get { return arrivalTime; }
            set
            {
                arrivalTime = value;
                OnPropertyChanged("ArrivalTime");
            }
        }
        public int TotalTimeRunned
        {
            get { return (CpuTimeRunned + AsynchronousTimeRunned + SynchronousTimeRunned); }
        }
        public int TotalTimeLeft
        {
            get { return (CpuTimeLeft + AsynchronousTimeLeft + SynchronousTimeLeft); }
        }
        public int CpuTimeRunned
        {
            get { return cpuTimeRunned; }
            set
            {
                cpuTimeRunned = value;
                OnPropertyChanged("CpuTimeRunned");
                OnPropertyChanged("TotalTimeRunned");
            }
        }
        public int AsynchronousTimeRunned
        {
            get { return asynchronousTimeRunned; }
            set
            {
                asynchronousTimeRunned = value;
                OnPropertyChanged("AsynchronousTimeRunned");
                OnPropertyChanged("TotalTimeRunned");
            }
        }
        public int SynchronousTimeRunned
        {
            get { return synchronousTimeRunned; }
            set
            {
                synchronousTimeRunned = value;
                OnPropertyChanged("SynchronousTimeRunned");
                OnPropertyChanged("TotalTimeRunned");
            }
        }
        public int TimeWaited
        {
            get { return timeWaited; }
            set
            {
                timeWaited = value;
                OnPropertyChanged("TimeWaited");
            }
        }
        public int CpuTimeLeft
        {
            get { return cpuTimeLeft; }
            set
            {
                cpuTimeLeft = value;
                OnPropertyChanged("CpuTimeLeft");
                OnPropertyChanged("TotalTimeLeft");
            }
        }
        public int AsynchronousTimeLeft
        {
            get { return asynchronousTimeLeft; }
            set
            {
                asynchronousTimeLeft = value;
                OnPropertyChanged("AsynchronousTimeLeft");
                OnPropertyChanged("TotalTimeLeft");
            }
        }
        public int SynchronousTimeLeft
        {
            get { return synchronousTimeLeft; }
            set
            {
                synchronousTimeLeft = value;
                OnPropertyChanged("SynchronousTimeLeft");
                OnPropertyChanged("TotalTimeLeft");
            }
        }
        public ObservableCollection<BurstDescriptor> BurstSequence
        {
            get { return burstSequence; }
            set
            {
                burstSequence = value;
                OnPropertyChanged("BurstSequence");
            }
        }

        public void ElpaseCpuTime()
        {
            CpuBurstDescriptor cpuBurst = getFirstCpuBurstIfPreviousSynchrBurstNotExists();
            if (cpuBurst != null)
            {
                cpuStep();

                cpuBurst.BurstTime -= 1;
                if (cpuBurst.BurstTime == 0)
                {
                    // cpu burst-re rákövetkező Asynchr. burstök és max 1 Synchr. burst
                    List<IoBurstDescriptor> initResourcesList = new List<IoBurstDescriptor>();
                    foreach (BurstDescriptor burst in BurstSequence)
                    {
                        if (burst.Equals(cpuBurst))
                            continue;

                        if (burst.GetType() == typeof(CpuBurstDescriptor))
                            break;

                        if (burst.GetType() == typeof(IoBurstSynchronousDescriptor))        // blokkolás
                        {
                            ProcessStatus = ProcessStatusEnum.Waiting;
                            initResourcesList.Add((IoBurstSynchronousDescriptor)burst);
                            break;
                        }
                        if (burst.GetType() == typeof(IoBurstAsynchronousDescriptor))
                        {
                            initResourcesList.Add((IoBurstAsynchronousDescriptor)burst);
                            continue;
                        }                        
                    }
                    simulatorModel.UseResources(this, initResourcesList);
                    BurstSequence.Remove(cpuBurst);
                }
            }
            if (BurstSequence.Count == 0)
            {
                ProcessStatus = ProcessStatusEnum.Finished;
                simulatorModel.Metrics.addTurnaroundTime(simulatorModel.ActualStep - arrivalTime);
                return;
            }

            if (BurstSequence.Where(x => x.GetType() == typeof(CpuBurstDescriptor)).Count() == 0)
            {
                ProcessStatus = ProcessStatusEnum.Ready;
                simulatorModel.Metrics.addTurnaroundTime(simulatorModel.ActualStep - arrivalTime);
                return;
            }
        }

        private CpuBurstDescriptor getFirstCpuBurstIfPreviousSynchrBurstNotExists()
        {
            // Első futtatható CPU burst megkeresése
            CpuBurstDescriptor firstCpuBurst = null;
            bool wasSynchrBurst = false;

            foreach (BurstDescriptor burst in BurstSequence)
            {
                if (burst.GetType() == typeof(CpuBurstDescriptor))
                {
                    firstCpuBurst = (CpuBurstDescriptor)burst;
                    break;
                }
                if (burst.GetType() == typeof(IoBurstSynchronousDescriptor))
                {
                    wasSynchrBurst = true;
                    break;
                }
            }

            // ha van előtte szinkron burst, akkor nincs futtatható
            if (wasSynchrBurst)
                return null;
            else
                return firstCpuBurst;
        }

        private void cpuStep()
        {
            CpuTimeRunned += 1;
            CpuTimeLeft -= 1;
        }

        public void resourceStep(IoBurstDescriptor ioBurst)
        {
            ioBurst.BurstTime -= 1;
            if (ioBurst.GetType() == typeof(IoBurstSynchronousDescriptor))
            {
                SynchronousTimeRunned += 1;
                SynchronousTimeLeft -= 1;
            }
            if (ioBurst.GetType() == typeof(IoBurstAsynchronousDescriptor))
            {
                AsynchronousTimeRunned += 1;
                AsynchronousTimeLeft -= 1;
            }

            if (ioBurst.BurstTime == 0)
            {
                BurstSequence.Remove(ioBurst);                                        // csak TESZT miatt (hogy látható legyen a blokkolás), élesnél vissza
                if (ioBurst.GetType() == typeof(IoBurstSynchronousDescriptor))
                {
                    ProcessStatus = ProcessStatusEnum.Ready;
                }
            }
            if (BurstSequence.Count == 0)
            {
                ProcessStatus = ProcessStatusEnum.Finished;
            }
        }

        public void Wait()
        {
            if (ProcessStatus != ProcessStatusEnum.Finished)
                TimeWaited += 1;
        }

    }
}

