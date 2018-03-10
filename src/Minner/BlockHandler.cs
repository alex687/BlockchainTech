using System.Linq;
using System.Threading.Tasks;
using Node.Core.Constants;
using Node.Core.Crypto;
using Node.Core.Extensions;
using Node.Core.Models;

namespace Minner
{
    public class BlockHandler
    {
        private readonly NodeCommunicator _communicationService;
        private readonly string _minerPublicKey;

        public BlockHandler(NodeCommunicator communicationService, string minerPublicKey)
        {
            _communicationService = communicationService;
            _minerPublicKey = minerPublicKey;
        }

        public async Task<Block> GetBlockToMine()
        {
            //var minnerAddress = _minerPublicKey.ComputeRipeMd160Hash();
            var minnerAddress = _minerPublicKey;
            var block = await _communicationService.GetBlockToMine(minnerAddress);

            var rewardAmount = block.Transactions
                .Where(e=>e.From != Genesis.MinerRewardSource)
                .CalculateReward();

            if (block.Transactions.Exists(t => t.From == Genesis.MinerRewardSource &&
                                               t.To == minnerAddress &&
                                               t.Amount >= rewardAmount))
            {
                return block;
            }

            return null;
        }

        public async Task<bool> SendMinnedBlock(Block block)
        {
            return await _communicationService.SendBlock(block);
        }
    }
}