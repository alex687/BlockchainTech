using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minner
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            var nodeUrl = ParseNodeUrl(args);
            var minerPublicKey = ParseMinerAddress(args);

            var logger = new Logger();
            var communicationService = new NodeCommunicator(nodeUrl);
            var rewardHandler = new BlockHandler(communicationService, minerPublicKey);
            var manager = new Manager(logger, rewardHandler);
            
            Task.WaitAll(manager.Start());
        }

        private static string ParseNodeUrl(IReadOnlyList<string> args)
        {
            if (args != null && args.Count > 0)
            {
                return args[0];
            }

            return "http://localhost:5000";
        }

        private static string ParseMinerAddress(IReadOnlyList<string> args)
        {
            if (args != null && args.Count > 1)
            {
                return args[1];
            }

            return "MinnerAddress";
        }
    }
}