using System.Collections.ObjectModel;

namespace PageReplacer.Models
{
    class PageReplacerNotRecentlyUsed : IPageReplacer
    {
        public PageReplacerNotRecentlyUsed(PR_SimulatorModel simulatorModel, ObservableCollection<string> parameters)
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
                    // ha tényleg nincs benne semmi, vissza kell adni legalább az igény és annak behozatalából összeállított rekordot
                    PageRecord clone = MyCloner.DeepClone<PageRecord>(pageRecord.CreateNewPageByPageNumberAndShiftTheOtherPages(i));
                    clone.setTimestampOnPage(i, simulatorModel.StepCounter);
                    return clone;
                }

                // ha már tartalmazza a kívánt lapot, akkor nem kell kivinni senkit
                if (pageRecord.containsPage(i))
                {
                    PageRecord clone = MyCloner.DeepClone<PageRecord>(pageRecord);
                    clone.setReferenceAndPageFault(i, false);
                    clone.setTimestampOnPage(i, simulatorModel.StepCounter);
                    return clone;
                }

                // ha nem tartalmazza a kívánt lapot az új lapot be kell vinni

                // ha van elég hely a tárban, a lapot beszúrjuk az első helyre
                if (pageRecord.hasMoreSpaceForNewPage())
                {
                    pageRecord.Pages.Insert(0, new Page(i));
                    pageRecord.setReferenceAndPageFault(i, true);
                    pageRecord.setTimestampOnPage(i, simulatorModel.StepCounter);
                    return pageRecord;
                }




                // ha nincs elég hely a tárban, áldozatot kell kiválasztani
                Page actualPage = null;

                foreach (Page page in pageRecord.Pages)
                {
                    if (page.Rbit == false && page.Mbit == false)
                    {
                        actualPage = page;
                    }
                }
                if (actualPage == null)
                {
                    foreach (Page page in pageRecord.Pages)
                    {
                        if (page.Rbit == false && page.Mbit == true)
                        {
                            actualPage = page;
                        }
                    }
                }
                if (actualPage == null)
                {
                    foreach (Page page in pageRecord.Pages)
                    {
                        if (page.Rbit == true && page.Mbit == false)
                        {
                            actualPage = page;
                        }
                    }
                }
                if (actualPage == null)
                {
                    foreach (Page page in pageRecord.Pages)
                    {
                        if (page.Rbit == true && page.Mbit == true)
                        {
                            actualPage = page;
                        }
                    }
                }

                // az áldozat helyére
                if (actualPage != null)
                {
                    pageRecord.ReplaceExistingPageWithNewPage(actualPage, i);
                    pageRecord.setTimestampOnPage(i, simulatorModel.StepCounter);
                    return pageRecord;
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
