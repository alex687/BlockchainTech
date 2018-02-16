using Node.Core.Factory;
using Node.Core.Repositories.Blockchain;

namespace Node.CLI
{
    public class BlockchainInstanceHolder
    {
        public BlockchainInstanceHolder(BlockchainFactory blockchainFactory)
        {
            Blockchain = blockchainFactory.CreateBlockchain();
        }

        public Blockchain Blockchain { get; set; }
    }
}