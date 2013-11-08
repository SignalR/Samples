using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Transports;
using WebApplication.Features.HubT;

namespace ConsoleClient.Features.HubT
{
    public class HubTClient : ITaskAgent, ITaskScheduler
    {
        private TextWriter _traceWriter;
        private IHubProxy _hubProxy;
        private int invocations = 5;
        private CountdownEvent cde;

        public HubTClient(TextWriter traceWriter)
        {
            _traceWriter = traceWriter;
        }

        public async Task RunAsync(string url)
        {
            try
            {
                await RunDemo(url);
            }
            catch (Exception exception)
            {
                _traceWriter.WriteLine("Exception: {0}", exception);
                throw;
            }
        }

        private async Task RunDemo(string url)
        {
            cde = new CountdownEvent(invocations);
            ITaskAgent client = this;
            ITaskScheduler server = this;

            var hubConnection = new HubConnection(url);
            hubConnection.TraceWriter = _traceWriter;

            _hubProxy = hubConnection.CreateHubProxy("TaskSchedulerHub");            
            _hubProxy.On<TimeSpan>("RunSync", client.RunSync);
            _hubProxy.On<TimeSpan>("RunAsync", (data) => client.RunAsync(data));

            await hubConnection.Start(new LongPollingTransport());

            var smallDuration = TimeSpan.FromMilliseconds(500);
            var largeDuration = TimeSpan.FromSeconds(10);

            for (int i = 0; i < invocations; i++ )
            {
                server.AssignMeShortRunningTask(smallDuration);
                server.AssignMeLongRunningTask(largeDuration);
            }

            cde.Wait();
        }

        private void WriteLine(string value)
        {
            string message = string.Format("{0} - {1}", DateTime.UtcNow.ToString("HH:mm:ss.fffffff"), value);
            _traceWriter.WriteLine(message);
        }

        void ITaskAgent.RunSync(TimeSpan duration)
        {
            WriteLine("Begin RunSync");
            Task.Delay(duration).Wait();
            WriteLine("Complete RunSync");
        }

        async Task ITaskAgent.RunAsync(TimeSpan duration)
        {
            WriteLine("Begin RunAsync");
            await Task.Delay(duration);
            WriteLine("Complete RunAsync");
            cde.Signal();
        }

        void ITaskScheduler.AssignMeShortRunningTask(TimeSpan duration)
        {
            _hubProxy.Invoke("AssignMeShortRunningTask", duration);
        }

        void ITaskScheduler.AssignMeLongRunningTask(TimeSpan duration)
        {
            _hubProxy.Invoke("AssignMeLongRunningTask", duration);
        }
    }
}
