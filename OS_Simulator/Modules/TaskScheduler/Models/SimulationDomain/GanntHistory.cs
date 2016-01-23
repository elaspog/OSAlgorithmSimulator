using Simulator.Infrastructure;
using System.Collections.ObjectModel;
using System.Linq;

namespace TaskScheduler.Models
{
    public class GanntHistory : Notifier
    {
        public GanntHistory()
        {
            processQueueCouples = new ObservableCollection<ProcessWithQueueAssigns>();
            queueAssigns = new ObservableCollection<QueueAssign>();


        }
        private ObservableCollection<ProcessWithQueueAssigns> processQueueCouples;
        private ObservableCollection<QueueAssign> queueAssigns;
        private int previousTime = -1;


        public ObservableCollection<ProcessWithQueueAssigns> ProcessQueueCouples
        {
            get { return processQueueCouples; }
            set
            {
                processQueueCouples = value;
                OnPropertyChanged("ProcessQueueCouples");
            }
        }
        public ObservableCollection<QueueAssign> QueueAssigns
        {
            get { return queueAssigns; }
            set
            {
                queueAssigns = value;
                OnPropertyChanged("QueueAssigns");
            }
        }
        
        public void AddToHistory(int time, Process executingProcess, ObservableCollection<Process> queue)
        {
            // hogy ne kelljen teljes Process-t emiatt sorosítani, ezért csak a neveket kiszedni
            ObservableCollection<string> queueWithProcessNames = new ObservableCollection<string>();
            foreach (Process process in queue)
            {
                queueWithProcessNames.Add(process.ProcessName);
            }

            string executingProcessName = "";
            if (executingProcess != null)
            {
                executingProcessName = executingProcess.ProcessName;
            }

            QueueAssign queueAssign = new QueueAssign(previousTime, time, queueWithProcessNames);


            if (ProcessQueueCouples.Count > 0)  // már van tárolva Gannt egység
            {
                ProcessWithQueueAssigns processWithQueueAssigns = ProcessQueueCouples.Last();

                if (processWithQueueAssigns.ProcessName != null)
                {
                    if (processWithQueueAssigns.ProcessName.Equals(executingProcessName))
                    {
                        if (processWithQueueAssigns.PartTimes.Count > 0)
                        {
                            if (CheckIfQuesAreEqual(queueWithProcessNames, processWithQueueAssigns.PartTimes.Last().Queue))
                            {
                                processWithQueueAssigns.setLastPartTimesEndInterval(time);
                            }
                            else
                            {
                                QueueAssigns.Add(queueAssign);
                                processWithQueueAssigns.addNewPartTime(queueAssign);
                            }
                        }
                        else
                        { 
                        
                        }
                    }
                    else
                    {
                        QueueAssigns.Add(queueAssign);
                        ObservableCollection<QueueAssign> partTime = new ObservableCollection<QueueAssign>();
                        partTime.Add(queueAssign);
                        processQueueCouples.Add(new ProcessWithQueueAssigns(executingProcessName, partTime));
                    }
                }
                else    // ha előzőleg nem volt tárolva process, ami végrehajtás alatt lett volna (tipikusan 2. iteráció, vagy esetleg sokadik) // több esetre kell szétválasztani
                {
                    QueueAssigns.Add(queueAssign);
                    ObservableCollection<QueueAssign> partTime = new ObservableCollection<QueueAssign>();
                    partTime.Add(queueAssign);
                    processQueueCouples.Add(new ProcessWithQueueAssigns(executingProcessName, partTime));
                }
            }
            else        // ha az adatszerkezetben még nincs korábban eltárolt Gannt egység  (1. iteráció)
            {
                QueueAssigns.Add(queueAssign);
                ObservableCollection<QueueAssign> partTime = new ObservableCollection<QueueAssign>();
                partTime.Add(queueAssign);
                processQueueCouples.Add(new ProcessWithQueueAssigns(executingProcessName, partTime));
            }
            previousTime = time;
        }


        private bool CheckIfQuesAreEqual(ObservableCollection<string> queue1, ObservableCollection<string> queue2)
        {
            if (queue1.Count != queue2.Count)
                return false;

            for (int i = 0; i < queue1.Count; i++)
            { 
                if ( ! queue1.ElementAt(i).Equals(queue2.ElementAt(i)) )
                {
                    return false;
                }
            }
            return true;
        }



    }
}
