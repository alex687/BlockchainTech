using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Node.Core.Constants;
using Node.Core.Models;

namespace Node.Core
{
    public class Blockchain : IBlockchain
    {
        private BlockingCollection<Block> _blocks;

        public Blockchain()
        {
            _blocks = new BlockingCollection<Block> {Genesis.Block};
        }

        public IEnumerable<Block> GetBlocks()
        {
            return _blocks.ToImmutableHashSet();
        }

        public Block GetBlock(int index)
        {
            if (index >= _blocks.Count || index < 0)
                throw new ArgumentException($"Index shoud be between 0 and {_blocks.Count - 1}", nameof(index));

            return _blocks.ElementAt(index);
        }

        public void AddBlock(Block block)
        {
            _blocks.TryAdd(block);
        }

        public void SyncBlocks(IEnumerable<Block> blocks)
        {
            var newBlock = new BlockingCollection<Block>();

            blocks.ToList().ForEach(AddBlock);

            _blocks = newBlock;
        }
    }
}