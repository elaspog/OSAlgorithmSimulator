using Simulator.Infrastructure;
using System.Collections.ObjectModel;

namespace TaskScheduler.Models
{
    public class Resource : Notifier
    {
        public Resource(ResourceDescriptor resourceDescriptor, TS_SimulatorModel simulatorModel)
        {
            queue = new ObservableCollection<ResourceRecord>();
            currentlyUsing = new ObservableCollection<ResourceRecord>();
            ResourceName = resourceDescriptor.ResourceName;
            Quantity = resourceDescriptor.Quantity;

            this.simulatorModel  = simulatorModel;
        }

        private string resourceName;
        public string ResourceName
        {
            get { return resourceName; }
            set 
            { 
                resourceName = value;
                OnPropertyChanged("ResourceName");
            }
        }

        private int quantity;
        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        private ObservableCollection<ResourceRecord> currentlyUsing;
        public ObservableCollection<ResourceRecord> CurrentlyUsing
        {
            get { return currentlyUsing; }
            set
            {
                currentlyUsing = value;
                OnPropertyChanged("currentlyUsing");
            }
        }

        private ObservableCollection<ResourceRecord> queue;
        public ObservableCollection<ResourceRecord> Queue
        {
            get { return queue; }
            set
            {
                queue = value;
                OnPropertyChanged("Queue");
            }
        }

        private TS_SimulatorModel simulatorModel;
        public TS_SimulatorModel SimulatorModel
        {
            get { return simulatorModel; }
            set
            {
                simulatorModel = value;
                OnPropertyChanged("SimulatorModel");
            }
        }


        // az operációs rendser az erőforrást a folyamathoz rendeli, vagy befüzi a folyamatot az erőforrás várakozási sorába
        public void addProcess(ResourceRecord resourceRecordToAdd)
        {
            bool addToCurrentlyUsing = true;
            if (CurrentlyUsing.Count >= Quantity)
            {
                addToCurrentlyUsing = false;
            }
            // ellenőrzés, hogy a folyamat aktívan használja-e már az erőforrást
            foreach (ResourceRecord record in CurrentlyUsing)
            {
                if (record.Process.ProcessName.Equals(resourceRecordToAdd.Process.ProcessName))
                {
                    addToCurrentlyUsing = false;
                }
            }

            if (addToCurrentlyUsing)
            {
                CurrentlyUsing.Add(resourceRecordToAdd);
            }
            else
            {
                Queue.Add(resourceRecordToAdd);
            }
        }

        
        public void ElpaseResourceTime()
        {
            ObservableCollection<ResourceRecord> processesToRemove = new ObservableCollection<ResourceRecord>(); 
            foreach (ResourceRecord record in CurrentlyUsing)
            {
                record.Process.resourceStep(record.Burst);
                if (record.Burst.BurstTime == 0)
                {
                    processesToRemove.Add(record);
                }
            }
            foreach (ResourceRecord resourceRecord in processesToRemove)
            {
                 CurrentlyUsing.Remove(resourceRecord);


                 if (resourceRecord.Burst.GetType() == typeof(IoBurstSynchronousDescriptor))
                 {
                     if (resourceRecord.Process.BurstSequence.Count > 0 )
                         SimulatorModel.Queue.Add(resourceRecord.Process);
                     
                 }
            }


            ObservableCollection<ResourceRecord> removeFromQueueAndAddedToCurrentlyUsing = new ObservableCollection<ResourceRecord>();
            foreach (ResourceRecord resourceRecord in Queue)
            {
                if (CurrentlyUsing.Count < Quantity)
                {
                    if (!CurrentlyUsing.Contains(resourceRecord))
                    {
                        bool containsSameProcess = false;
                        foreach (ResourceRecord record in CurrentlyUsing)
                        {
                            if (record.Process.Equals(resourceRecord.Process))
                            {
                                containsSameProcess = true;
                                break;
                            }
                        }
                        if (!containsSameProcess)
                        {
                            CurrentlyUsing.Add(resourceRecord);
                            removeFromQueueAndAddedToCurrentlyUsing.Add(resourceRecord);
                        }
                    } // nem kell Break, mert lehet hogy a sorban az első nem párosítható, mivel ugyanaz a process már használja kor´äbbról az erőforrást
                }
                else
                {
                    break;
                }
            }
            foreach (ResourceRecord record in removeFromQueueAndAddedToCurrentlyUsing)
            {
                Queue.Remove(record);
            }


        }

        
    }
}
