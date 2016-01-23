using System;
using System.Collections.ObjectModel;

namespace PageReplacer.Models
{
    public class PageReplacerOptimal : IPageReplacer
    {
        public PageReplacerOptimal(PR_SimulatorModel simulatorModel, ObservableCollection<string> parameters)
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


                // minden lapra a laptáblában meg kell nézni, mikor lesz a következő akció, amiben szerepel
                int notNeedMaxTime = 0;
                Page pageTmp = null;

                foreach (Page page in pageRecord.Pages)
                {
                    bool pageWasInRequestList = false;

                    foreach (PageActionBase action in simulatorModel.PageActionSequence)
                    {
                        // ha a lap benne van a listában, ideiglenesen eltesszük
                        if (action.Page == page.PageNumber)
                        {
                            pageWasInRequestList = true;

                            int notNeedTimeTmp = simulatorModel.PageActionSequence.IndexOf(action);

                            if (notNeedTimeTmp > notNeedMaxTime)
                            {
                                notNeedMaxTime = notNeedTimeTmp;
                                pageTmp = page;
                            }
                            break;
                        }
                    }

                    // ha a lap nem volt a várakozási listában, ki lehet vinni, többé nincs rá szükség
                    if (pageWasInRequestList == false) 
                    {
                        pageTmp = page;
                        break;
                    }
                }

                // Mostanra már biztosan talált egy kivihető lapot
                if (pageTmp != null)
                {
                    int index = pageRecord.Pages.IndexOf(pageTmp);
                    PageRecord clone = MyCloner.DeepClone<PageRecord>(pageRecord);
                    clone.ReplaceExistingPageWithNewPage(pageTmp, i);
                    clone.setTimestampOnPage(i, simulatorModel.StepCounter);
                    return clone;
                }
                // elvileg ide nem juthat az algoritmus
                throw new NotImplementedException();
                


            }
            return null;
        }

        public bool usesPeriodsToRemoveRbits()
        {
            return false;
        }
    }
}
