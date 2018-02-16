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
    public class Blockchain
    {
        private readonly BlockingCollection<Block> _blocks;

        private readonly TransactionsRepository _transactionsRepository;
        private readonly PendingTransactionRepository _pendingTransactionRepository;

        private readonly IBlockValidator _blockValidator;
        private readonly ITransactionValidator _transactionValidator;

        public Blockchain(TransactionsRepository transactionsRepository, PendingTransactionRepository pendingTransactionRepository, IBlockValidator blockValidator, ITransactionValidator transactionValidator)
        {
            _transactionsRepository = transactionsRepository;
            _pendingTransactionRepository = pendingTransactionRepository;

            _blockValidator = blockValidator;
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

                var invalidTransaction = new List<PendingTransaction>();
                foreach (var pendingTransaction in _pendingTransactionRepository.GetPending())
                {
                    if (!_transactionValidator.Validate(pendingTransaction))
                    {
                        invalidTransaction.Add(pendingTransaction);
                    }
                }

                _pendingTransactionRepository.RemovePending(invalidTransaction);

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
            var pendingTransactions = _pendingTransactionRepository.GetPending().ToList();
            pendingTransactions.Add(transaction);

            if (_transactionValidator.PendingTransactionsValidate(pendingTransactions))
            {
                return _pendingTransactionRepository.AddPending(transaction);
            }

            return false;
        }
    }
}