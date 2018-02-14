using System.Collections.Generic;
using Node.Core.Models;

namespace Node.Core
{
    public interface IBlockchain
    {
        IEnumerable<Block> GetBlocks();

        Block GetBlock(int index);

        void AddBlock(Block block);

        void SyncBlocks(IEnumerable<Block> blocks);
    }
}
