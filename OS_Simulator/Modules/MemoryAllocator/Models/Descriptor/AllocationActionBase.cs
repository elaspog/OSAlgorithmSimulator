using Simulator.Infrastructure;
using System;

namespace MemoryAllocator.Models
{
    abstract public class AllocationActionBase: Notifier, ICloneable
    {
        public AllocationActionBase()
        {
                
        }


        public object Clone()
        {
            return (AllocationActionBase)this.MemberwiseClone();
        }
    }
}
