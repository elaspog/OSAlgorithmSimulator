using Simulator.Infrastructure;
using Simulator.Infrastructure.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PageReplacer.Models
{
    public class PR_SimulatorModel : Notifier, ISimulator
    {
        public PR_SimulatorModel()
        {
            pageTable = new PageTable();
            pageActionSequence = new ObservableCollection<PageActionBase>();
            stepCounter = 0;
        }

        private PageTable pageTable;
        public PageTable PageTable
        {
            get { return pageTable; }
            set
            {
                pageTable = value;
                OnPropertyChanged("PageTable");
            }
        }

        private ObservableCollection<PageActionBase> pageActionSequence;
        public ObservableCollection<PageActionBase> PageActionSequence
        {
            get { return pageActionSequence; }
            set
            {
                pageActionSequence = value;
                OnPropertyChanged("PageActionSequence");
            }
        }

        private Metrics metrics;
        public Metrics Metrics
        {
            get { return metrics; }
            set
            {
                metrics = value;
                OnPropertyChanged("Metrics");
            }
        }

        public int MemoryAccessTime
        {
            get { return Metrics.MemoryAccessTime; }
        }

        public int PageFaultServiceTime
        {
            get { return Metrics.PageFaultServiceTime; }
        }

        private int countOfPagesForProcess;
        public int CountOfPagesForProcess
        {
            get { return countOfPagesForProcess; }
            set
            {
                countOfPagesForProcess = value;
                OnPropertyChanged("CountOfPagesForProcess");
            }
        }

        private IPageReplacer pageReplacer = null;
        public Type PageReplacerType
        {
            get 
            {
                if (pageReplacer == null)
                    return null;
                return pageReplacer.GetType();
            }
        }


        int stepCounter;
        public int StepCounter
        {
            get { return stepCounter; }
            set
            {
                stepCounter = value;
                OnPropertyChanged("StepCounter");
            }
        }

        public bool NextStep()
        {
            StepCounter++;

            if (PageActionSequence.Count > 0)
            {
                PageActionBase action = PageActionSequence.ElementAt(0);

                PageRecord oldPageRecord = GetLastPageRecord();

                if (action.GetType() == typeof(PageAccess))
                {
                    // csak akkor fordulhat elő, ha még a History üres, tehát az első lapnak üre rekordot kell generálni
                    if (oldPageRecord == null)
                    {
                        oldPageRecord = new PageRecord(countOfPagesForProcess);
                    }
                    // az utolsó lap lap és az igény függvényében az algoritmus új "bejegyzést" generál (nem lapszámot ad vissza!!!)
                    PageRecord newPageRecord = pageReplacer.processPageRequestAndReturnNewPageRecordAccordingToHistory(((PageAccess)action).Page, MyCloner.DeepClone<PageRecord>(oldPageRecord));

                    if (newPageRecord != null)
                    {
                        metrics.setMetrics(newPageRecord);
                        // új bejegyzés bekerül a History-ba
                        pageTable.PageRecords.Insert(0, MyCloner.DeepClone<PageRecord>(newPageRecord));
                        PageActionSequence.RemoveAt(0);
                        return true;
                    }
                    else { return false; }
                }

                if (action.GetType() == typeof(PageSetMbit) || action.GetType() == typeof(PageSetRbit)
                    || action.GetType() == typeof(PageRemoveMbit) || action.GetType() == typeof(PageRemoveRbit))
                {
                    if (oldPageRecord == null)
                    {
                        return false;
                    }
                    PageRecord newPageRecord = MyCloner.DeepClone<PageRecord>(oldPageRecord);

                    if (newPageRecord.containsPage(((PageActionBase)action).Page))
                    {
                        if (action.GetType() == typeof(PageSetMbit))
                        {
                            newPageRecord.setMbit(((PageActionBase)action).Page, true);
                        }
                        if (action.GetType() == typeof(PageSetRbit))
                        {
                            newPageRecord.setRbit(((PageActionBase)action).Page, true);
                        }
                        if (action.GetType() == typeof(PageRemoveMbit))
                        {
                            newPageRecord.setMbit(((PageActionBase)action).Page, false);
                        }
                        if (action.GetType() == typeof(PageRemoveRbit))
                        {
                            newPageRecord.setRbit(((PageActionBase)action).Page, false);
                        }
                        newPageRecord.Referenced = action.Page;
                        newPageRecord.PageFault = false;
                        pageTable.PageRecords.Insert(0, MyCloner.DeepClone<PageRecord>(newPageRecord));
                        PageActionSequence.RemoveAt(0);
                    }
                    else
                    {
                        newPageRecord.Referenced = action.Page;
                        newPageRecord.PageFault = true;
                        pageTable.PageRecords.Insert(0, MyCloner.DeepClone<PageRecord>(newPageRecord));
                        PageActionSequence.RemoveAt(0);
                    }
                }


                if (action.GetType() == typeof(PeriodRemoveAllRbit))
                {
                    PageRecord newPageRecord = MyCloner.DeepClone<PageRecord>(oldPageRecord);

                    if (!pageReplacer.usesPeriodsToRemoveRbits())
                    {
                        foreach (Page page in newPageRecord.Pages)
                        {
                            page.Rbit = false;
                        }
                        newPageRecord.Referenced = null;
                        newPageRecord.PageFault = false;
                    }
                    else 
                    {
                        newPageRecord = ((IPageReplacerEvent)pageReplacer).reactOnPeriodicalRbitRemovalEvent(MyCloner.DeepClone<PageRecord>(newPageRecord));
                    }
                    pageTable.PageRecords.Insert(0, MyCloner.DeepClone<PageRecord>(newPageRecord));
                    PageActionSequence.RemoveAt(0);
                }

                return false;
            }
            return false;
        }

        public bool PreviousStep()
        {
            throw new NotImplementedException();
        }

        public void CreateSimulatorDomain(PR_Descriptor descriptor)
        {
            PageActionSequence = new ObservableCollection<PageActionBase>(descriptor.PageActionSequence.Select(i => (PageActionBase)i.Clone()).ToList());
            CountOfPagesForProcess = descriptor.CountOfPagesForProcess;
            Metrics = new Metrics(descriptor.MemoryAccessTime,descriptor.PageFaultServiceTime);
            LoadPageFaultAlgorithm(descriptor.PageReplacerAlgorithm);
        }

        private void LoadPageFaultAlgorithm(PageReplacerAlgorithm pageReplacerAlgorithm)
        {
            switch (pageReplacerAlgorithm.AlgorithmName.ToUpper())
            {
                case "OPTIMAL":
                case "OPT":
                    pageReplacer = new PageReplacerOptimal(this, pageReplacerAlgorithm.Parameters);
                    break;
                case "FIRST INPUT FIRST OUTPUT":
                case "FIRST_INPUT_FIRST_OUTPUT":
                case "FIRSTINPUTFIRSTOUTPUT":
                case "FIFO":
                    pageReplacer = new PageReplacerFifo(this, pageReplacerAlgorithm.Parameters);
                    break;
                case "SECOND CHANCE":
                case "SECOND_CHANCE":
                case "SECONDCHANCE":
                case "SC":
                    pageReplacer = new PageReplacerSecondChance(this, pageReplacerAlgorithm.Parameters);
                    break;
                case "LAST RECENTLY USED":
                case "LAST_RECENTLY_USED":
                case "LASTRECENTLYUSED":
                case "LRU":
                    pageReplacer = new PageReplacerLastRecentlyUsed(this, pageReplacerAlgorithm.Parameters);
                    break;
                case "LAST FREQUENTLY USED":
                case "LAST_FREQUENTLY_USED":
                case "LASTFREQUENTLYUSED":
                case "LFU":
                case "NOT FREQUENTLY USED":
                case "NOT_FREQUENTLY_USED":
                case "NOTFREQUENTLYUSED":
                case "NFU":
                    pageReplacer = new PageReplacerLastFrequentlyUsed(this, pageReplacerAlgorithm.Parameters);
                    break;
                case "NOT RECENTLY USED":
                case "NOT_RECENTLY_USED":
                case "NOTRECENTLYUSED":
                case "NRU":
                    pageReplacer = new PageReplacerNotRecentlyUsed(this, pageReplacerAlgorithm.Parameters);
                    break;

            }
        }


        private PageRecord GetLastPageRecord()
        {
            if (PageTable.PageRecords.Count > 0)
                return PageTable.PageRecords.First();
            return null;
        }

    }
}
