using System;
using System.Collections.Generic;
using System.Linq;
using Node.Core.Constants;
using Node.Core.Models;
using Node.Core.Validators.Block;
using Node.Core.Validators.Transactions;

namespace Node.Core
{
    public class Blockchain : IBlockchain
    {
        private readonly IBlockValidator _blockValidator;
        private readonly ITransactionValidator _transactionValidator;

        private List<Block> _blocks;
        private List<Transaction> _pendingTransactions;

        public Blockchain(IBlockValidator blockValidator, ITransactionValidator transactionValidator)
        {
            _blockValidator = blockValidator;
            _transactionValidator = transactionValidator;

            _blocks = new List<Block> { Genesis.Block };
            _pendingTransactions = new List<Transaction>();
        }

        public IEnumerable<Block> GetBlocks()
        {
            //TODO: clone the blocks
            return _blocks;
        }

        public Block GetBlock(int index)
        {
            if (index >= _blocks.Count || index < 0)
            {
                throw new ArgumentException($"Index shoud be between 0 and {_blocks.Count - 1}", nameof(index));
            }

            //TODO: clone block
            return _blocks[index];
        }

        public bool AddBlock(Block block)
        {
            var lastBlock = _blocks[_blocks.Count - 1];
            if (!_blockValidator.ValidateBlock(block, lastBlock))
            {
                return false;
            }

            _blocks.Add(block);
            return true;
        }

        public long GetBalance(string address, int confirmations)
        {
            var accountTransactions = _blocks
                .Take(_blocks.Count - confirmations)
                .SelectMany(b => b.Transactions);

            var from = accountTransactions.Where(t => t.From == address).Sum(t => t.Amount);
            var to = accountTransactions.Where(t => t.To == address).Sum(t => t.Amount);

            return to - from;
        }

        public bool AddPendingTransaction(Transaction transaction)
        {
            if (!_transactionValidator.ValidateTransaction(transaction))
            {
                return false;
            }

            _pendingTransactions.Add(transaction);
            return true;
        }

        public Transaction GetTransaction(string transactionHash)
        {
            return _blocks.SelectMany(b => b.Transactions).FirstOrDefault(t => t.Hash == transactionHash);
        }

        public void SyncBlocks(IEnumerable<Block> blocks)
        {
            _blocks = blocks.ToList();
        }
    }
}
