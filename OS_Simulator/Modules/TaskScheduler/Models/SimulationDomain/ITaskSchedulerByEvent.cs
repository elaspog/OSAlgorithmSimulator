
namespace TaskScheduler.Models
{
    interface ITaskSchedulerByEvent : ITaskScheduler
    {
        void update();

        bool needToRun();
    }
}
