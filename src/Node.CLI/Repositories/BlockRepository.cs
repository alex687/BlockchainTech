using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Node.Core.Constants;
using Node.Core.Models;

namespace Node.CLI.Repositories
{
    public class BlockRepository
    {
        private BlockingCollection<Block> _blocks;

        public BlockRepository()
        {
            _blocks = new BlockingCollection<Block> {Genesis.Block};
        }

        public IEnumerable<Block> GetBlocks()
        {
            return _blocks.ToImmutableList();
        }

        public Block GetBlock(int index)
        {
            return _blocks.ElementAt(index);
        }

        public void AddBlock(Block block)
        {
            _blocks.TryAdd(block);
        }

        public int GetBlockCount()
        {
            return _blocks.Count;
        }

        public void SyncBlocks(IEnumerable<Block> blocks)
        {
            var newBlock = new BlockingCollection<Block>();
            blocks.ToList().ForEach(AddBlock);
            _blocks = newBlock;
        }
    }
}