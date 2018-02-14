using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Node.Core.Constants;
using Node.Core.Models;

namespace Node.Core
{
    public class Blockchain : IBlockchain
    {
        private List<Block> _blocks;

        public Blockchain()
        {
            _blocks = new List<Block> {Genesis.Block};
        }

        public IEnumerable<Block> GetBlocks()
        {
            return _blocks.ToImmutableArray();
        }

        public Block GetBlock(int index)
        {
            if (index >= _blocks.Count || index < 0)
                throw new ArgumentException($"Index shoud be between 0 and {_blocks.Count - 1}", nameof(index));

            return _blocks[index];
        }

        public void AddBlock(Block block)
        {
            _blocks.Add(block);
        }

        public void SyncBlocks(IEnumerable<Block> blocks)
        {
            _blocks = blocks.ToList();
        }
    }
}