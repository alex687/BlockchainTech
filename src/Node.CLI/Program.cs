﻿using System;
using System.Net;
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

            string command = string.Empty;
            while (command != "exit")
            {
                Console.Write(">");
                command = Console.ReadLine()?.ToLower();
            }
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel(o => o.Listen(IPAddress.Loopback, 0))
                .UseKestrel()
                .Build();
        }
    }
}