using Simulator.Infrastructure;
using Simulator.Infrastructure.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace VirtualAddressMapper.Models
{
    public enum NumberBase
    {
        Binary, Decimal, Hexadecimal
    }

    public class VAM_SimulatorModel : Notifier, ISimulator
    {
        public VAM_SimulatorModel()
        {
            NumberBase = Models.NumberBase.Decimal;
        }

        public VAM_SimulatorModel(VAM_Descriptor InputDescriptor)
        {
            this.CreateSimulatorDomain(InputDescriptor);
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
        public enum State { Ready, Processsing1, Processsing2, Processsing3, Processsing4, Finished };
        public State state;


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

        private AddressingExtended addressing;
        public AddressingExtended Addressing
        {
            get { return addressing; }
            set
            {
                addressing = value;
                OnPropertyChanged("Addressing");
            }
        }

        private ObservableCollection<ActionBase> actionSequence;
        public ObservableCollection<ActionBase> ActionSequence
        {
            get { return actionSequence; }
            set
            {
                actionSequence = value;
                OnPropertyChanged("ReferencedVirtualAddressSequence");
            }
        }

        private ObservableCollection<MappingRecordOfProcessAndMemory> processPageAndMemoryPageMapping;
        public ObservableCollection<MappingRecordOfProcessAndMemory> ProcessPageAndMemoryPageMapping
        {
            get { return processPageAndMemoryPageMapping; }
            set
            {
                processPageAndMemoryPageMapping = value;
                OnPropertyChanged("ProcessPageAndMemoryPageMapping");
                OnPropertyChanged("ProcessPageList");
                OnPropertyChanged("MemoryFrameList");
            }
        }

        public ObservableCollection<IndicatorRecord> ProcessPageList
        {
            get
            {
                ObservableCollection<IndicatorRecord> processList = new ObservableCollection<IndicatorRecord>();
                for (int i = 0; i < Addressing.NumberOfPages; i++)
                {
                    processList.Add(new IndicatorRecord(i, false));
                }
                foreach (MappingRecordOfProcessAndMemory record in ProcessPageAndMemoryPageMapping)
                {
                    processList.ElementAt(record.ProcessPage).Flag = true;
                }
                return processList;
            }
        }

        public ObservableCollection<IndicatorRecord> MemoryFrameList
        {
            get
            {
                ObservableCollection<IndicatorRecord> memoryList = new ObservableCollection<IndicatorRecord>();
                for (int i = 0; i < Addressing.QuantityOfPagesStorableInUserMemory; i++)
                {
                    memoryList.Add(new IndicatorRecord(null, i, false));
                }
                foreach (MappingRecordOfProcessAndMemory record in ProcessPageAndMemoryPageMapping)
                {
                    memoryList.ElementAt(record.MemoryFrame).Flag = true;
                    memoryList.ElementAt(record.MemoryFrame).Page = record.ProcessPage;
                }
                return memoryList;
            }
        }


        private ObservableCollection<FourElementTuple> associativeMemory;
        public ObservableCollection<FourElementTuple> AssociativeMemory
        {
            get { return associativeMemory; }
            set
            {
                associativeMemory = value;
                OnPropertyChanged("VirtualAddressStr");
            }
        }

        public void CreateSimulatorDomain(VAM_Descriptor descriptor)
        {
            StepCounter = 0;
            
            Addressing = new AddressingExtended(descriptor.Addressing);

            Metrics = new Metrics(Addressing.AssociativeMemoryAccessTime, Addressing.MemoryAccessTime);

            ObservableCollection<MappingRecordOfProcessAndMemory> addList = new ObservableCollection<MappingRecordOfProcessAndMemory>();
            foreach (MappingRecordOfProcessAndMemory record in descriptor.ProcessPageAndMemoryPageMapping)
            {
                bool addItem = true;

                if (record.ProcessPage >= Addressing.NumberOfPages || record.ProcessPage < 0)
                {
                    addItem = false;
                    continue;
                }
                if (record.MemoryFrame >= Addressing.QuantityOfPagesStorableInUserMemory || record.MemoryFrame < 0)
                {
                    addItem = false;
                    continue;
                }
                for (int i = addList.Count - 1; i > 0; i--)
                {
                    if (record.MemoryFrame == addList.ElementAt(i).MemoryFrame)
                    {
                        addItem = false;
                        break;
                    }
                    if (record.ProcessPage == addList.ElementAt(i).ProcessPage)
                    {
                        addItem = false;
                        break;
                    }
                }
                if (addItem)
                {
                    addList.Add(record);
                }
            }
            ProcessPageAndMemoryPageMapping = MyCloner.DeepClone<ObservableCollection<MappingRecordOfProcessAndMemory>>(addList);


            // associativ memóriába csak azok a page addressek mennek be, akikhez tartozó process lap a memóriában van
            AssociativeMemory = new ObservableCollection<FourElementTuple>();
            if (descriptor.Addressing.IsAssociativeMemoryInUse)
            {
                foreach (int processPage in descriptor.AssociativeMemoryInitialContent)
                {
                    AddPageToAssociativeMemory(processPage);
                }
            }

            // ellenőrzéssel kell
            ActionSequence = MyCloner.DeepClone<ObservableCollection<ActionBase>>(descriptor.ActionSequence);
            ObservableCollection<ActionBase> removeList = new ObservableCollection<ActionBase>();
            foreach (ActionBase action in ActionSequence)
            {
                string fullName = action.GetType().Name;
                switch (fullName)
                {
                    case "ResolveVirtualAddress":
                        break;
                    case "AddPageToMemory":
                        if (((AddPageToMemory)action).ProcessPage >= Addressing.NumberOfPages
                            || ((AddPageToMemory)action).ProcessPage < 0
                            || ((AddPageToMemory)action).MemoryFrame >= Addressing.QuantityOfPagesStorableInUserMemory
                            || ((AddPageToMemory)action).MemoryFrame < 0)
                        {
                            removeList.Add(action);
                        }
                        break;
                    case "RemovePageFromMemory":
                        if (((RemovePageFromMemory)action).MemoryFrame >= Addressing.QuantityOfPagesStorableInUserMemory
                            || ((RemovePageFromMemory)action).MemoryFrame < 0)
                        {
                            removeList.Add(action);
                        }
                        break;
                    case "AddPageToAssociativeMemory":
                        if (descriptor.Addressing.IsAssociativeMemoryInUse == false)
                        {
                            removeList.Add(action);
                        }
                        break;
                    case "RemovePageFromAssociativeMemory":
                        if (descriptor.Addressing.IsAssociativeMemoryInUse == false)
                        {
                            removeList.Add(action);
                        }
                        break;
                }
            }
            foreach (ActionBase action in removeList)
            {
                ActionSequence.Remove(action);
            }

        }
        

        private bool associativeMemoryContainsProcessPage(int processPage)
        {
            foreach (FourElementTuple record in AssociativeMemory)
            {
                if (record.MappingRecordOfProcessAndMemory.ProcessPage == processPage)
                {
                    return true;
                }
            }
            return false;
        }

        private int itemsLeft = 0;
        private int itemsLeftPeriodCounter = 0;

        public bool NextStep()
        {
            switch (state)
            {
                case State.Ready:

                    if (ActionSequence != null && ActionSequence.Count != 0)
                    {
                        string fullName = ActionSequence.ElementAt(0).GetType().Name;
                        int page;

                        switch (fullName)
                        {

                            case "ResolveVirtualAddress":

                                VirtualAddress = new VeryLongNumberByNumberBaseInString(((ResolveVirtualAddress)ActionSequence.ElementAt(0)).Address.ToString(), 10);
                                state = State.Processsing1;

                                break;


                            case "AddPageToMemory":

                                AddPageToMemory pageToAdd = (AddPageToMemory)ActionSequence.ElementAt(0);

                                if (pageToAdd.ProcessPage >= 0 && pageToAdd.ProcessPage < Addressing.NumberOfPages)
                                {
                                    bool isOccupied = false;
                                    foreach (MappingRecordOfProcessAndMemory mapping in ProcessPageAndMemoryPageMapping)
                                    {
                                        if (mapping.MemoryFrame == pageToAdd.MemoryFrame
                                            || mapping.ProcessPage == pageToAdd.ProcessPage)
                                        {
                                            isOccupied = true;
                                            break;
                                        }
                                    }
                                    if (!isOccupied)
                                    {
                                        ProcessPageAndMemoryPageMapping.Add(new MappingRecordOfProcessAndMemory(pageToAdd.ProcessPage, pageToAdd.MemoryFrame));
                                    }
                                }
                                break;


                            case "RemovePageFromMemory":

                                RemovePageFromMemory pageToRemove = (RemovePageFromMemory)ActionSequence.ElementAt(0);

                                MappingRecordOfProcessAndMemory itemToRemove = null;
                                foreach (MappingRecordOfProcessAndMemory mapping in ProcessPageAndMemoryPageMapping)
                                {
                                    if (mapping.MemoryFrame == pageToRemove.MemoryFrame)
                                    {
                                        itemToRemove = mapping;
                                    }
                                }
                                if (itemToRemove != null)
                                {
                                    ProcessPageAndMemoryPageMapping.Remove(itemToRemove);
                                }
                                // törölni kell majd az asszociatív memóriából is
                                ObservableCollection<FourElementTuple> listToRemove = new ObservableCollection<FourElementTuple>();
                                foreach (FourElementTuple record in AssociativeMemory)
                                {
                                    if (record.MappingRecordOfProcessAndMemory.MemoryFrame == pageToRemove.MemoryFrame)
                                    {
                                        listToRemove.Add(record);
                                    }
                                }
                                if (listToRemove != null)
                                {
                                    foreach (FourElementTuple rec in listToRemove)
                                    {
                                        AssociativeMemory.Remove(rec);
                                    }
                                }
                                break;


                            case "AddPageToAssociativeMemory":

                                page = ((AddPageToAssociativeMemory)ActionSequence.ElementAt(0)).ProcessPage;
                                AddPageToAssociativeMemory(page);
  
                                break;


                            case "RemovePageFromAssociativeMemory":

                                page = ((RemovePageFromAssociativeMemory)ActionSequence.ElementAt(0)).ProcessPage;
                                RemovePageToAssociativeMemory(page);

                                break;

                        }
                        ActionSequence.RemoveAt(0);
                    }
                    break;

                case State.Processsing1:

                    string virtAddressBin = VirtualAddress.getNumberInBase(2);

                    if (virtAddressBin.Count() <= Addressing.AddressingBits)
                    {

                        virtAddressBin = completeAddress(Addressing.AddressingBits, virtAddressBin);

                        PageAddress = new VeryLongNumberByNumberBaseInString(virtAddressBin.Substring(0, Addressing.BitsToAddressPages), 2);
                        DistanceOnPage = new VeryLongNumberByNumberBaseInString(virtAddressBin.Substring(Addressing.BitsToAddressPages, Addressing.BitsToAddressOnPage), 2);
                    }
                    else
                    {
                        AddressSourcePageTable = "Segm. Err.";
                        state = State.Finished;
                        break;
                    }

                    state = State.Processsing2;

                    break;


                case State.Processsing2:

                    state = State.Processsing3;
                    AddressSourceAssociative = "No";

                    if (Addressing.IsAssociativeMemoryInUse)
                    {
                        foreach (FourElementTuple record in AssociativeMemory)
                        {
                            if (LargeStringNumberComparator.areEqual(record.MappingRecordOfProcessAndMemory.ProcessPage.ToString(), PageAddress.getNumberInBase(10)))
                            {
                                AddressSourceAssociative = "Yes";
                                FrameAddress = new VeryLongNumberByNumberBaseInString(record.FrameAddress.ToString(), 10);
                                Metrics.makeAssociativeMemoryHit();
                                break;
                            }
                        }
                    }
                    if (AddressSourceAssociative.Equals("Yes"))
                        break;
                    
                    bool isVirtAddressOutOfRange = LargeStringNumberComparator.isFirstGreater(VirtualAddress.getNumberInBase(10), Addressing.ProcessAddressRangeByLastAddress.Address.ToString());
                    bool isVirtAddressUnderZero  = LargeStringNumberComparator.isFirstGreater("0", VirtualAddress.getNumberInBase(10));

                    if (isVirtAddressOutOfRange || isVirtAddressUnderZero)
                    {
                        state = State.Finished;
                        AddressSourcePageTable = "Segm. Err.";
                        break;
                    }


                    AddressSourcePageTable = "PageFault";
                    foreach (MappingRecordOfProcessAndMemory record in ProcessPageAndMemoryPageMapping)
                    {

                        int processPage = Addressing.getProcessPageByVirtualAddress(VirtualAddress.getNumberInBase(10));

                        if (record.ProcessPage.Equals(processPage))
                        {
                            AddressSourcePageTable = "Yes";                            
                            FrameAddress = new VeryLongNumberByNumberBaseInString(Addressing.generateFrameAddress(record).ToString(), 10);

                            Metrics.makePageTableAndAddressHit();
                            break;
                        }
                    }
                    if (AddressSourcePageTable.Equals("PageFault"))
                    {
                        state = State.Finished;
                    }


                    break;

                case State.Processsing3:

                    PhysicalAddress =  new VeryLongNumberByNumberBaseInString(LargeStringNumberComparator.getSum(FrameAddress.getNumberInBase(10), DistanceOnPage.getNumberInBase(10)), 10);

                    state = State.Processsing4;

                    break;

                case State.Processsing4:
                    
                    VirtualAddress = PageAddress = DistanceOnPage = FrameAddress = PhysicalAddress = null;
                    AddressSourceAssociative = AddressSourcePageTable = "";
                    state = State.Ready;

                    break;

                case State.Finished:
                    break;


            }

            OnPropertyChanged("ProcessPageList");
            OnPropertyChanged("MemoryFrameList");

            if (itemsLeft == ActionSequence.Count)
            {
                itemsLeftPeriodCounter++;
            }
            else
            {
                itemsLeftPeriodCounter = 0;
            }
            itemsLeft = ActionSequence.Count;

            if (itemsLeftPeriodCounter > 4)
            {
                return false;
            }
            else
            {
                stepCounter++;
                return true;
            }

        }

        private void AddPageToAssociativeMemory(int processPage)
        {
            
            foreach (MappingRecordOfProcessAndMemory record in ProcessPageAndMemoryPageMapping)
            {
                if (processPage == record.ProcessPage
                    && AssociativeMemory.Count < Addressing.QuantityOfAddressesStorableInAssociativeMemory
                    && !associativeMemoryContainsProcessPage(processPage))
                {
                    int generatedPageAddress = Addressing.generatePageAddress(record);
                    int generatedFrameAddress = Addressing.generateFrameAddress(record);
                    AssociativeMemory.Add(new FourElementTuple(generatedPageAddress, generatedFrameAddress, record));

                    break;
                }
            }
        }

        private void RemovePageToAssociativeMemory(int page)
        {
            ObservableCollection<FourElementTuple> removeList = new ObservableCollection<FourElementTuple>();
            if (associativeMemoryContainsProcessPage(page))
            {
                foreach (FourElementTuple record in AssociativeMemory)
                {
                    if (record.MappingRecordOfProcessAndMemory.ProcessPage.Equals(page))
                    {
                        removeList.Add(record);
                    }
                }
            }
            foreach (FourElementTuple record in removeList)
            {
                AssociativeMemory.Remove(record);
            }
        }

        public bool PreviousStep()
        {
            throw new NotImplementedException();
        }
        
        string completeAddress(int toSize, string part)
        {
            int partSize;
            if (part != null)
            {
                partSize = part.Count();
            }
            else
            {
                return "";
            }

            if (partSize == toSize)
            {
                return part;
            }

            if (partSize > toSize)
            {
                return "";
            }

            string appendString = "";
            for (int i = 0; i < toSize - partSize; i++)
            {
                appendString += "0";
            }

            return appendString + part;
        }
        
        
        private NumberBase numberBase;
        public NumberBase NumberBase
        {
            get { return numberBase; }
            set
            {
                numberBase = value;
                OnPropertyChanged("NumberBase");
                OnPropertyChanged("VirtualAddressStr");
                OnPropertyChanged("PageAddressStr");
                OnPropertyChanged("DistanceOnPageStr");
                OnPropertyChanged("FrameAddressStr");
                OnPropertyChanged("PhysicalAddressStr");
            }
        }

        private string getNumberByActualNumberBase(VeryLongNumberByNumberBaseInString value)
        {
            if (value != null)
            {
                if (NumberBase == NumberBase.Binary)
                {
                    return value.getNumberInBase(2);
                }
                if (NumberBase == NumberBase.Decimal)
                {
                    return value.getNumberInBase(10);
                }
                if (NumberBase == NumberBase.Hexadecimal)
                {
                    return value.getNumberInBase(16);
                }
            }
            return null;
        }


        private VeryLongNumberByNumberBaseInString virtualAddress;
        public VeryLongNumberByNumberBaseInString VirtualAddress
        {
            get { return virtualAddress; }
            set
            {
                virtualAddress = value;
                OnPropertyChanged("VirtualAddress");
                OnPropertyChanged("VirtualAddressStr");
            }
        }
        public string VirtualAddressStr
        {
            get
            {
                if (NumberBase == NumberBase.Binary)
                {
                    return completeAddress(Addressing.AddressingBits, getNumberByActualNumberBase(virtualAddress));
                } 
                return getNumberByActualNumberBase(virtualAddress);
            }
        }
        

        private VeryLongNumberByNumberBaseInString pageAddress;
        public VeryLongNumberByNumberBaseInString PageAddress
        {
            get { return pageAddress; }
            set
            {
                pageAddress = value;
                OnPropertyChanged("PageAddress");
                OnPropertyChanged("PageAddressStr");
            }
        }
        public string PageAddressStr
        {
            get
            {
                if (NumberBase == NumberBase.Binary)
                {
                    return completeAddress(Addressing.BitsToAddressPages, getNumberByActualNumberBase(pageAddress));
                }
                return getNumberByActualNumberBase(pageAddress); 
            }
        }


        private VeryLongNumberByNumberBaseInString distanceOnPage;
        public VeryLongNumberByNumberBaseInString DistanceOnPage
        {
            get { return distanceOnPage; }
            set
            {
                distanceOnPage = value;
                OnPropertyChanged("DistanceOnPage");
                OnPropertyChanged("DistanceOnPageStr");
            }
        }
        public string DistanceOnPageStr
        {
            get
            {
                if (NumberBase == NumberBase.Binary)
                {
                    return completeAddress(Addressing.BitsToAddressOnPage, getNumberByActualNumberBase(distanceOnPage));
                }
                return getNumberByActualNumberBase(distanceOnPage);
            }
        }


        private VeryLongNumberByNumberBaseInString frameAddress;
        public VeryLongNumberByNumberBaseInString FrameAddress
        {
            get { return frameAddress; }
            set
            {
                frameAddress = value;
                OnPropertyChanged("FrameAddress");
                OnPropertyChanged("FrameAddressStr");
            }
        }
        public string FrameAddressStr
        {
            get
            {
                if (NumberBase == NumberBase.Binary)
                {
                    return completeAddress(Addressing.BitsToAddressPages, getNumberByActualNumberBase(frameAddress));
                }
                return getNumberByActualNumberBase(frameAddress);
            }
        }


        private VeryLongNumberByNumberBaseInString physicalAddress;
        public VeryLongNumberByNumberBaseInString PhysicalAddress
        {
            get { return frameAddress; }
            set
            {
                physicalAddress = value;
                OnPropertyChanged("PhysicalAddress");
                OnPropertyChanged("PhysicalAddressStr");
            }
        }
        public string PhysicalAddressStr
        {
            get
            {
                if (NumberBase == NumberBase.Binary)
                {
                    return completeAddress(Addressing.AddressingBits, getNumberByActualNumberBase(physicalAddress));
                }
                return getNumberByActualNumberBase(physicalAddress);
            }
        }
        
        private string addressSourceAssociative;
        public string AddressSourceAssociative
        {
            get { return addressSourceAssociative; }
            set
            {
                addressSourceAssociative = value;
                OnPropertyChanged("AddressSourceAssociative");
            }
        }
        private string addressSourcePageTable;
        public string AddressSourcePageTable
        {
            get { return addressSourcePageTable; }
            set
            {
                addressSourcePageTable = value;
                OnPropertyChanged("AddressSourcePageTable");
            }
        }


    }
}
