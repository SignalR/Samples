using System;
using System.Threading.Tasks;

namespace WebApplication.Features.HubT
{
    public interface ITaskAgent
    {
        void RunSync(TimeSpan duration);
        Task RunAsync(TimeSpan duration);
    }
}