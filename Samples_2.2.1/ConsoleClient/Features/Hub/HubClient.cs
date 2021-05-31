using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace ConsoleClient.Features.Hub
{
    class HubClient
    {
        private TextWriter _traceWriter;
        private IHubProxy _hubProxy;

        public HubClient(TextWriter traceWriter)
        {
            _traceWriter = traceWriter;
        }

        public async Task RunAsync(string url)
        {
            var connection = new HubConnection(url);
            connection.TraceWriter = _traceWriter;

            _hubProxy = connection.CreateHubProxy("DemoHub");

            _hubProxy.On<string>("hubMessage", (data) =>
            {
                _traceWriter.WriteLine("hubMessage: " + data);
            });

            await connection.Start();
            await _hubProxy.Invoke("SendToMe", "Hello World!");
        }
    }
}
