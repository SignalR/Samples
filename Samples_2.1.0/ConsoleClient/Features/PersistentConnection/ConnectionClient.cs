using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace ConsoleClient.Features.PersistentConnection
{
    class ConnectionClient
    {
        private TextWriter _traceWriter;
        private Connection _connection;

        public ConnectionClient(TextWriter traceWriter)
        {
            _traceWriter = traceWriter;
        }

        public async Task RunAsync(string url)
        {
            _connection = new Connection(url);
            _connection.TraceWriter = _traceWriter;
            _connection.Received += (data) =>
            {
                _traceWriter.WriteLine("received: " + data);
            };

            await _connection.Start();
            await _connection.Send(new { Type = "sendToMe", Content = "Hello World!" });
        }
    }
}
