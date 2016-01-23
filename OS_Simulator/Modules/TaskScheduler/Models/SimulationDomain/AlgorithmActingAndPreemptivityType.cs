
namespace TaskScheduler.Models
{
    public enum AlgorithmActingAndPreemptivityType
    {
        None,               // FIFO, SJF
        ByTimeSlice,        // RR
        ByArrivedProcess,   // SRTF
    }
}
