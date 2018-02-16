using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Node.Core.Models;

namespace Node.Core.Repositories.Blockchain
{
    public class TransactionsRepository
    {
        private readonly ConcurrentDictionary<string, Transaction> _transactions;
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Transaction>> _addressTransactions;

        public TransactionsRepository()
        {
            _transactions = new ConcurrentDictionary<string, Transaction>();
            _addressTransactions = new ConcurrentDictionary<string, ConcurrentDictionary<string, Transaction>>();
        }

        public List<Transaction> GetTransactions(string address)
        {
            if (!_addressTransactions.ContainsKey(address))
            {
                return new List<Transaction>();
            }

            var transactions = _addressTransactions[address];
            return transactions.Values.ToList();
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