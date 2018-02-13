using System;
using System.Collections.Generic;
using Node.Core.Models;

namespace Node.Core
{
     public class Blockchain : IBlockchain
    {
        public List<Block> GetBlocks()
        {
            throw new NotImplementedException();
        }

        public Block GetBlock(int index)
        {
            throw new NotImplementedException();
        }

        public bool AddBlock(Block block)
        {
            throw new NotImplementedException();
        }

        public long GetBalance(Address address)
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
    }
}
