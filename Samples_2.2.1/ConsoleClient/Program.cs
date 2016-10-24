using System;
using ConsoleClient.Features.Hub;
using ConsoleClient.Features.HubT;
using ConsoleClient.Features.PersistentConnection;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write(
@"
New Features in version 1.0.0
  1. PersistentConnection
  2. Hub
New Features in version 2.1.0
  3. Hub<T>

Select sample you want to run: ");
            var sample = Console.ReadKey().KeyChar;
            Console.WriteLine();

            var url = "http://localhost:46962/";
            var writer = Console.Out;

            switch(sample)
            {
                case '1':
                    var client = new ConnectionClient(writer);
                    client.RunAsync(url + "Connections/DemoPersistentConnection").Wait();
                    break;
                case '2':
                    var hubClient = new HubClient(writer);
                    hubClient.RunAsync(url).Wait();
                    break;
                case '3':
                    var hubTClient = new HubTClient(writer);
                    hubTClient.RunAsync(url).Wait();
                    break;
                default:
                    break;
            }

            Console.WriteLine("Sample completed. Press ENTER to finish program.");
            Console.ReadLine();
        }
    }
}
