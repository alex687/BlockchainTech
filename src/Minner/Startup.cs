using System;

namespace Minner
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            var nodeUrl = "http://localhost:5000";
            var minerAddress = "MinnerAddress";

            var communicationService = new Communication(nodeUrl, minerAddress);

            var manager = new Manager(communicationService);

            manager.Start();

            while (true)
            {
                var command = Console.ReadLine();
            }
        }
    }
}