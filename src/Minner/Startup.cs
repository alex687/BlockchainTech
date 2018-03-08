using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minner
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            var nodeUrl = GetNodeUrl();
            var minerPublicKey = GetMinerAddress();

            var logger = new Logger();
            var communicationService = new NodeCommunicator(nodeUrl);
            var rewardHandler = new BlockHandler(communicationService, minerPublicKey);
            var manager = new Manager(logger, rewardHandler);
            
            Task.WaitAll(manager.Start());
        }

        private static string GetNodeUrl()
        {
            Console.WriteLine("Node url:");
            var miner = Console.ReadLine();

            return miner;
        }

        private static string GetMinerAddress()
        {
            Console.WriteLine("Miner wallet address:");
            var address = Console.ReadLine();

            return address;
        }
    }
}