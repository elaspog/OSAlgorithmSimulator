using System;
using System.Numerics;
namespace VirtualAddressMapper.Models
{
    public class AddressingExtended : Addressing
    {
        public AddressingExtended(Addressing addressing) 
            : base(addressing)
        {

        }

        public int NumberOfRecordsInPageTable
        {
            get { return (int)Math.Pow(2, BitsToAddressPages); }
        }

        public int NumberOfAddressesOnPage
        {
            get { return (int)Math.Pow(2, BitsToAddressOnPage); }
        }

        public int NumberOfPages
        {
            get { return 1 + (int)Math.Floor((double)ProcessAddressRangeByLastAddress.Address / (double)NumberOfAddressesOnPage); }
        }

        public int InnerFragmentation
        {
            get { return ((NumberOfPages * NumberOfAddressesOnPage) - 1 - ProcessAddressRangeByLastAddress.Address); }
        }

                
        public int getProcessPageByVirtualAddress(int virtualAddress)
        {
            return (int)Math.Floor((double)virtualAddress / (double)NumberOfAddressesOnPage);
        }

        public int getProcessPageByVirtualAddress(string virtualAddress)
        {
            BigInteger virtAddress;
            BigInteger.TryParse(virtualAddress, out virtAddress);

            double num;

            double.TryParse(BigInteger.Divide(virtAddress, NumberOfAddressesOnPage).ToString(),out num);

            return (int)Math.Floor(num);
            
        }


        public int generateFrameAddress(MappingRecordOfProcessAndMemory record)
        {
            int memoryAddress = record.MemoryFrame * NumberOfAddressesOnPage;

            return memoryAddress;
        }

        public int generatePageAddress(MappingRecordOfProcessAndMemory record)
        {
            int memoryAddress = record.ProcessPage * NumberOfAddressesOnPage;

            return memoryAddress;
        }

    }
}
