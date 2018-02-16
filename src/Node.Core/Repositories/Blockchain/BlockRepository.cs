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

        public Transaction GetTransaction(string hash)
        {
            return _transactionsRepository.GetTransaction(hash);
        }

        public IEnumerable<PendingTransaction> GetPending()
        {
            return _pendingTransactionRepository.GetPending();
        }

        public bool AddPending(PendingTransaction transaction)
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