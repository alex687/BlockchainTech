using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Node.CLI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var restApi = BuildWebHost(args);
            Task.Run(() => { restApi.Run(); });
            
            while (true)
            {
                var command = Console.ReadLine();

            }
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel()
                .Build();
        }
    }
}