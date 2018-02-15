using Node.Core.Factory;
using Node.Core.Repositories.Blockchain;

namespace Node.CLI
{
    public class BlockchainInstanceHolder
    {
        public BlockchainInstanceHolder(BlockchainFactory blockchainFactory)
        {
            BlockRepository = blockchainFactory.CreateBlockchain();
        }

        public BlockRepository BlockRepository { get; set; }
    }
}