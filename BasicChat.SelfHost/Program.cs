using System;
using Microsoft.Owin.Hosting;

namespace BasicChat.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:8080";

            using (WebApplication.Start<Startup>(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }
    }
}
