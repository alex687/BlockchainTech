using System;
using System.Collections.Generic;
using Node.Core.Models;

namespace Node.Core
{
    public class Blockchain : IBlockchain
    {
        private readonly List<Block> _blocks;

        public Blockchain()
        {
            _blocks = new List<Block>();
        }

        public List<Block> GetBlocks()
        {
            throw new NotImplementedException();
        }

        public Block GetBlock(int index)
        {
            if (index >= _blocks.Count || index < 0)
            {
                throw  new  ArgumentException($"Index shoud be between 0 and {_blocks.Count - 1}", nameof(index));
            }
            return _blocks[index];
        }

        public bool AddBlock(Block block)
        {
            _blocks.Add(block);

            return true;
        }

        public long GetBalance(string address)
        {
            throw new NotImplementedException();
        }

        public bool AddPendingTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public Transaction GetTransaction(string transactionHash)
        {
            throw new NotImplementedException();
        }

        public bool SyncBlocks(IEnumerable<Block> blocks)
        {
            throw new NotImplementedException();
        }
    }
}
