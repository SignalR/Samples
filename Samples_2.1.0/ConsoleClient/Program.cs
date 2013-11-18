using System;
using ConsoleClient.Features.HubT;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write(
@"
New Features in SignalR 1.0.0
  1. PersistentConnection
  2. Hub
New Features in SignalR 1.0.1
New Features in SignalR 1.1.0
New Features in SignalR 1.1.1
New Features in SignalR 1.1.2
New Features in SignalR 1.1.3
New Features in SignalR 2.0.0
New Features in SignalR 2.1.0
  1. Hub<T>

Select sample you want to run: ");
            var sample = Console.ReadKey().KeyChar;
            Console.WriteLine();

            var url = "http://localhost:46962/";
            var writer = Console.Out;

            switch(sample)
            {
                case '1':
                    var client = new HubTClient(writer);
                    client.RunAsync(url).Wait();
                    break;
                default:
                    break;
            }

            Console.WriteLine("Sample completed. Press ENTER to finish program.");
            Console.ReadLine();
        }
    }
}
