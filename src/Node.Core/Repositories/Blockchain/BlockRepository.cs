using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Node.Core.Constants;
using Node.Core.Models;
using Node.Core.Validators.Block;
using Node.Core.Validators.Transactions;

namespace Node.Core.Repositories.Blockchain
{
    public class BlockRepository
    {
        private readonly BlockingCollection<Block> _blocks;

        private readonly TransactionsRepository _transactionsRepository;
        private readonly IBlockValidator _blockValidator;
        private readonly PendingTransactionRepository _pendingTransactionRepository;
        private readonly ITransactionValidator _transactionValidator;

        public BlockRepository(TransactionsRepository transactionsRepository, IBlockValidator blockValidator, PendingTransactionRepository pendingTransactionRepository, ITransactionValidator transactionValidator)
        {
            _transactionsRepository = transactionsRepository;
            _blockValidator = blockValidator;
            _pendingTransactionRepository = pendingTransactionRepository;
            _transactionValidator = transactionValidator;
            _blocks = new BlockingCollection<Block> { Genesis.Block };
            _transactionsRepository.AddTransaction(Genesis.Block.Transactions);
        }

        public IEnumerable<Block> GetBlocks()
        {
            return _blocks.ToImmutableList();
        }

        public Block GetBlock(int index)
        {
            return _blocks.ElementAt(index);
        }
      
        public bool TryAddBlock(Block block)
        {
            var prevBlock = _blocks.Last();
            var isValid = _blockValidator.Validate(block, prevBlock);
            if (isValid)
            {
                _blocks.Add(block);

                _transactionsRepository.AddTransaction(block.Transactions);
                _pendingTransactionRepository.RemovePending(block.Transactions);

                return true;
            }

            return false;
        }

        public decimal GetBalance(string address, int confirmations)
        {
            var addressTransactions = _transactionsRepository.GetTransactions(address).Where(t => t.BlockIndex <= _blocks.Count - confirmations).ToList();

            var from = addressTransactions
                .Where(t => t.From == address)
                .Sum(t => t.Amount);

            var to = addressTransactions
                .Where(t => t.To == address)
                .Sum(t => t.Amount);

            return to - from;
        }


        /*public bool TrySyncBlocks(IEnumerable<Block> blocks)
        {
            if (HasBiggerWeigth(blocks))
            {
                var transactionsRepository = new TransactionsRepository();
                var blockValidator = new BlockValidator(new TransactionValidator(transactionsRepository));

                var previous = blocks.Take(1).First();
                foreach (var block in blocks.Skip(1))
                {
                    if (!blockValidator.Validate(block, previous))
                    {
                        return false;
                    }

                    previous = block;
                }

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
            _transactionsRepository.AddTransaction(minedTransactions);
        }

        private void UpdateTransactionsCache(IEnumerable<Block> blocks)
        {
            _transactionsRepository.ReloadCache(blocks);
        }

        private bool HasBiggerWeigth(IEnumerable<Block> newBlocks)
        {
            var currentBlocks = _blocks;
            var newBlockWeight = newBlocks.Sum(e => e.Difficulty);
            var oldBlockWeight = currentBlocks.Sum(e => e.Difficulty);

            return newBlockWeight > oldBlockWeight ||
                   newBlockWeight == oldBlockWeight && newBlocks.Count() > currentBlocks.Count();
        }*/
        public Transaction GetTransaction(string hash)
        {
            return _transactionsRepository.GetTransaction(hash);
        }


        public bool AddPending(Transaction transaction)
        {
            if (_transactionValidator.Validate(transaction))
            {
                _pendingTransactionRepository.AddPending(transaction);

                return true;
            }

            return false;
        }
    }
}