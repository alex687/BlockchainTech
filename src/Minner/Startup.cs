using System;
using System.Threading.Tasks;

namespace Minner
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter nodeUrl");
            //var nodeUrl = "http://localhost:5000";
            var nodeUrl = Console.ReadLine();
            var minerAddress = "MinnerAddress";

            var logger = new Logger();

            var communicationService = new NodeCommunicator(logger, nodeUrl, minerAddress);

            var manager = new Manager(logger, communicationService);

            var task = manager.Start();

            Task.WaitAll(task);
        }
    }
}