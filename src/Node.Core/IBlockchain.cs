using System.Collections.Generic;
using Node.Core.Models;

namespace Node.Core
{
    public interface IBlockchain
    {
        List<Block> GetBlocks();

        Block GetBlock(int index);

        bool AddBlock(Block block);

        long GetBalance(string address);

        bool AddPendingTransaction(Transaction transaction);

        Transaction GetTransaction(string transactionHash);

        bool SyncBlocks(IEnumerable<Block> blocks);
    }
}
