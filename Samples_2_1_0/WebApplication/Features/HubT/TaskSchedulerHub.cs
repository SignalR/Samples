using System;
using Microsoft.AspNet.SignalR;

namespace WebApplication.Features.HubT
{
    public class TaskSchedulerHub : Hub<ITaskAgent>, ITaskScheduler
    {
        public void AssignMeShortRunningTask(TimeSpan duration)
        {
            Clients.Caller.RunSync(duration);
        }

        public void AssignMeLongRunningTask(TimeSpan duration)
        {
            Clients.Caller.RunAsync(duration);
        }
    }
}