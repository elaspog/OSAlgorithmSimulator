
namespace TaskScheduler.Models
{
    interface ITaskSchedulerByTime : ITaskScheduler
    {
        void updateTime(int ms);

        bool needToRun();
    }
}
