using System;

namespace WebApplication.Features.HubT
{
    public interface ITaskScheduler
    {
        void AssignMeShortRunningTask(TimeSpan duration);
        void AssignMeLongRunningTask(TimeSpan duration);
    }
}