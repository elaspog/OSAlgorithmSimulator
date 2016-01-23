using System.Collections.ObjectModel;
using System.Linq;

namespace PageReplacer.Models
{
    public class PageReplacerSecondChance : IPageReplacer
    {
        public PageReplacerSecondChance(PR_SimulatorModel simulatorModel, ObservableCollection<string> parameters)
        {
            this.simulatorModel = simulatorModel;
        }

        PR_SimulatorModel simulatorModel;
        public PR_SimulatorModel SimulatorModel
        {
            get { return simulatorModel; }
            set { simulatorModel = value; }
        }

        public PageRecord processPageRequestAndReturnNewPageRecordAccordingToHistory(int i, PageRecord pageRecord)
        {
            if (pageRecord != null)
            {
                // csak a elgelső iterációban lehet üres, amikor még a History is üres
                if (pageRecord.Pages.Count == 0)
                {
                    // ha tényleg nincs benne semmi, vissza kell adni legalább az igényt és annak behozatalából összeállított rekordot
                    PageRecord clone = MyCloner.DeepClone<PageRecord>(pageRecord.CreateNewPageByPageNumberAndShiftTheOtherPages(i));
                    clone.setTimestampOnPage(i, simulatorModel.StepCounter);
                    return clone;
                }

                // ha már legalább egy lap van a tárban
                // megnézni, hogy a lap a tárban van-e
                bool wasPageInMemory = false;
                foreach (Page page in pageRecord.Pages)
                {
                    if (page.PageNumber == i)
                    {
                        // a keresett lapot nem kell behozni 
                        wasPageInMemory = true;
                        break;
                    }
                }

                // ha a lap nem volt a tárban
                if (!wasPageInMemory)
                {
                    if (pageRecord.hasMoreSpaceForNewPage())
                    {
                        pageRecord.Pages.Insert(0, new Page(i));
                        pageRecord.setTimestampOnPage(i, simulatorModel.StepCounter);
                        pageRecord.setReferenceAndPageFault(i, true);
                        return pageRecord;
                    }

                    ObservableCollection<Page> pagesToAddToList = new ObservableCollection<Page>();
                    Page victim = null;

                    // áldozat kiválasztása ha van, egyben a 
                    foreach (Page page in pageRecord.Pages.Reverse())
                    {
                        if (page.Rbit == true)
                        {
                            page.Rbit = false;
                            pagesToAddToList.Add(page);
                        }
                        else
                        {
                            victim = page;
                            break;
                        }
                    }

                    // csak akkor lehet ha minden lap a laptáblán R-bites volt és victim nem került kiválasztásra
                    if (pagesToAddToList.Count == pageRecord.Pages.Count && victim == null)
                    {
                        pageRecord.Pages.Remove(pageRecord.Pages.Last());
                        pageRecord.Pages.Insert(0, new Page(i));
                        pageRecord.setReferenceAndPageFault(i, true);
                        pageRecord.setTimestampOnPage(i, simulatorModel.StepCounter);
                        return MyCloner.DeepClone<PageRecord>(pageRecord); 
                    }

                    // ha a laptáblán kevesebb lap került kiválasztásra mint amennyi benne volt akkor volt benne áldozat is
                    // ezért minden lapot előre beszúrni
                    if (pagesToAddToList != null)  
                    {
                        foreach (Page page in pagesToAddToList)
                        {
                            pageRecord.Pages.Remove(page);
                            pageRecord.Pages.Insert(0, page);
                        }
                    }
                    // majd az áldozatot eltakarítani, helyébe a kért lapot behozni
                    if (pageRecord.containsPage(victim.PageNumber))
                    {
                        pageRecord.Pages.Remove(victim);
                        pageRecord.Pages.Insert(0, new Page(i));
                    }

                    pageRecord.setTimestampOnPage(i, simulatorModel.StepCounter);
                    pageRecord.setReferenceAndPageFault(i, true);
                    return MyCloner.DeepClone<PageRecord>( pageRecord);

                }
                else
                {
                    // különben meg nem volt laphiba
                    PageRecord clone = MyCloner.DeepClone<PageRecord>(pageRecord);
                    clone.setReferenceAndPageFault(i, false);
                    clone.setTimestampOnPage(i, simulatorModel.StepCounter);
                    return clone;
                }
            }
            return null;
        }


        public bool usesPeriodsToRemoveRbits()
        {
            return false;
        }
    }
}
