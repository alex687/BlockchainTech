using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Node.Core.Models;

namespace Node.CLI.Repositories.Caches
{
    public class TransactionCache
    {
        private readonly BlockRepository _blockRepository;
        private readonly ConcurrentDictionary<string, Transaction> _transactions;
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Transaction>> _addressTransactions;

        public TransactionCache(BlockRepository blockRepository)
        {
            _blockRepository = blockRepository;
            _transactions = new ConcurrentDictionary<string, Transaction>();
            _addressTransactions = new ConcurrentDictionary<string, ConcurrentDictionary<string, Transaction>>();
        }

        public List<Transaction> GeTransactions(string address, int confirmations)
        {
            var transactions = _addressTransactions[address];
            var blockCount = _blockRepository.GetBlockCount();

            var confirmedTransactions =
                transactions.Values.Where(t => t.BlockIndex <= blockCount - confirmations).ToList();
            return confirmedTransactions;
        }

        public Transaction GetTransaction(string hash)
        {
            return _transactions[hash];
        }

        public void AddTransaction(IEnumerable<Transaction> minedTransactions)
        {
            foreach (var minedTransaction in minedTransactions)
            {
                UpdateDictionaries(minedTransaction);
            }
        }

        public void ReloadCache(IEnumerable<Block> blocks)
        {
            _transactions.Clear();
            _addressTransactions.Clear();

            foreach (var block in blocks)
            {
                block.Transactions.ForEach(UpdateDictionaries);
            }
        }

        private void UpdateDictionaries(Transaction transaction)
        {
            var fromAccountTransactions = _addressTransactions.GetOrAdd(transaction.From, new ConcurrentDictionary<string, Transaction>());
            fromAccountTransactions.TryAdd(transaction.Hash, transaction);

            var toAccountTransactions = _addressTransactions.GetOrAdd(transaction.To, new ConcurrentDictionary<string, Transaction>());
            toAccountTransactions.TryAdd(transaction.Hash, transaction);

            _transactions.TryAdd(transaction.Hash, transaction);
        }
    }
}