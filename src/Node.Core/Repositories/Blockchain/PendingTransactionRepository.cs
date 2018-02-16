using System.Collections.Concurrent;
using System.Collections.Generic;
using Node.Core.Models;

namespace Node.Core.Repositories.Blockchain
{
    public class PendingTransactionRepository
    {
        private readonly ConcurrentDictionary<string, PendingTransaction> _pending;

        public PendingTransactionRepository()
        {
            _pending = new ConcurrentDictionary<string, PendingTransaction>();
        }

        public void AddPending(PendingTransaction transaction)
        {
            _pending.TryAdd(transaction.Hash, transaction);
        }

        public void RemovePending(IEnumerable<PendingTransaction> minedTrans)
        {
            foreach (var mined in minedTrans)
            {
                _pending.TryRemove(mined.Hash, out _);
            }
        }

        public IEnumerable<PendingTransaction> GetPending()
        {
            return _pending.Values;
        }
    }
}