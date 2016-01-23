using System.Collections.ObjectModel;

namespace PageReplacer.Models
{
    public class PageReplacerFifo : IPageReplacer
    {
        public PageReplacerFifo(PR_SimulatorModel simulatorModel, ObservableCollection<string> parameters)
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
                // csak a legelső iterációban lehet üres, amikor még a History is üres
                if (pageRecord.Pages.Count == 0)
                {
                    // ha tényleg nincs benne semmi, vissza kell adni legalább az igény és annak behozatalából összeállított rekordot
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
                    // a keresett lapot be kell hozni, a tárból az utolsót kivinni, laphibát generálni
                    PageRecord clone = MyCloner.DeepClone<PageRecord>(pageRecord.CreateNewPageByPageNumberAndShiftTheOtherPages(i));
                    clone.setTimestampOnPage(i, simulatorModel.StepCounter);
                    return clone;
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
