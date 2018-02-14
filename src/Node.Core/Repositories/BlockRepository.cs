using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Node.Core.Caches;
using Node.Core.Constants;
using Node.Core.Models;
using Node.Core.Validators.Block;
using Node.Core.Validators.Transactions;

namespace Node.Core.Repositories
{
    public class BlockRepository
    {
        private BlockingCollection<Block> _blocks;
        private readonly TransactionCache _transactionsCache;
        private readonly IBlockValidator _blockValidator;

        public BlockRepository(IBlockValidator blockValidator, TransactionCache transactionCache)
        {
            _transactionsCache = transactionCache;
            _blockValidator = blockValidator;
            _blocks = new BlockingCollection<Block> { Genesis.Block };
            transactionCache.AddTransaction(Genesis.Block.Transactions);
        }

        public IEnumerable<Block> GetBlocks()
        {
            return _blocks.ToImmutableList();
        }

        public Block GetBlock(int index)
        {
            return _blocks.ElementAt(index);
        }

        public decimal GetBalance(string address, int confirmations)
        {
            var addressTransactions = _transactionsCache.GeTransactions(address).Where(t => t.BlockIndex <= _blocks.Count - confirmations).ToList();

            var from = addressTransactions
                .Where(t => t.From == address)
                .Sum(t => t.Amount);

            var to = addressTransactions
                .Where(t => t.To == address)
                .Sum(t => t.Amount);

            return to - from;
        }

        public bool TryAddBlock(Block block)
        {
            var prevBlock = _blocks.Last();
            var isValid = _blockValidator.Validate(block, prevBlock);
            if (isValid)
            {
                _blocks.Add(block);
                UpdateTransactionsCache(block);

                return true;
            }

            return false;
        }

        public int GetBlockCount()
        {
            return _blocks.Count;
        }

        public bool TrySyncBlocks(IEnumerable<Block> blocks)
        {
            if (HasBiggerWeigth(blocks))
            {
                var newBlock = new BlockingCollection<Block>();
                _blocks = newBlock;
                UpdateTransactionsCache(blocks);

                return true;
            }

            return false;
        }

        private void UpdateTransactionsCache(Block block)
        {
            var minedTransactions = block.Transactions;
            _transactionsCache.AddTransaction(minedTransactions);
        }

        private void UpdateTransactionsCache(IEnumerable<Block> blocks)
        {
            _transactionsCache.ReloadCache(blocks);
        }

        private bool HasBiggerWeigth(IEnumerable<Block> newBlocks)
        {
            var currentBlocks = _blocks;
            var newBlockWeight = newBlocks.Sum(e => e.Difficulty);
            var oldBlockWeight = currentBlocks.Sum(e => e.Difficulty);

            return newBlockWeight > oldBlockWeight ||
                   newBlockWeight == oldBlockWeight && newBlocks.Count() > currentBlocks.Count();
        }
    }
}