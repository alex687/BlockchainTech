using System.Collections.Concurrent;
using System.Collections.Generic;
using Node.Core.Models;

namespace Node.Core.Repositories
{
    public class PendingTransactionRepository
    {
        private readonly ConcurrentDictionary<string, Transaction> _pending;

        public PendingTransactionRepository()
        {
            _pending = new ConcurrentDictionary<string, Transaction>();
        }

        public void AddPending(Transaction transaction)
        {
            _pending.TryAdd(transaction.Hash, transaction);
        }

        public void RemovePending(IEnumerable<Transaction> minedTrans)
        {
            foreach (var mined in minedTrans)
            {
                _pending.TryRemove(mined.Hash, out _);
            }
        }

        public IEnumerable<Transaction> GetPending()
        {
            return _pending.Values;
        }
    }
}