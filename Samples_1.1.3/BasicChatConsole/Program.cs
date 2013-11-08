using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace BasicChatConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        private static async Task RunAsync()
        {
            var connection = new HubConnection("http://localhost:44914/");
            IHubProxy chat = connection.CreateHubProxy("Chat");

            chat.On<string>("send", Console.WriteLine);

            await connection.Start();

            string line = null;
            await Task.Factory.StartNew(() =>
            {
                while (!String.IsNullOrEmpty(line = Console.ReadLine()))
                {
                    chat.Invoke("send", "Console: " + line);
                }
            });
        }
    }
}
