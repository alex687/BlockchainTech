using Node.Core.Repositories.Blockchain;

namespace Node.Core.Factory
{
    public interface IBlockchainFactory
    {
        Blockchain CreateBlockchain();
    }
}