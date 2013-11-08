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
Samples SignalR 2.1.0
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
