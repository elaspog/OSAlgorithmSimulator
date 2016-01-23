
namespace TaskScheduler.Models
{
    interface ITaskScheduler
    {
        Process get ();

        AlgorithmActingAndPreemptivityType GetActingAndPreemptivityType();
    }
}
