using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace WebApplication.Features.HubT
{
    public class LongRunningTaskHub : Hub
    {
        public async Task<string> RunAsync(string jobName, IProgress<ProgressUpdate> progress)
        {
            var delay = TimeSpan.FromMilliseconds(100);
            var progressUpdate = new ProgressUpdate();

            for (int i = 0; i <= 100; i++)
            {
                await Task.Delay(delay);
                progressUpdate.Percent = i;
                progressUpdate.LastUpdate = DateTime.UtcNow;
                if( i < 50)
                {
                    progressUpdate.Message = "less than half the work is done";
                }
                else
                {
                    progressUpdate.Message = "almost completing the work";
                }
                progress.Report(progressUpdate);
            }

            return String.Format("{0} done!", jobName);
        }
    }

    public class ProgressUpdate
    {
        public int Percent { get; set; }
        public DateTime LastUpdate { get; set; }
        public string Message { get; set; }
    }
}