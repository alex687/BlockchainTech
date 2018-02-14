using System.Collections.Concurrent;
using System.Collections.Generic;
using Node.Core.Models;

namespace Node.CLI.Repositories
{
    public class TransactionRepository
    {
        private readonly ConcurrentDictionary<string, Transaction> _pending;

        public TransactionRepository(BlockRepository blockRepo)
        {
            _pending = new ConcurrentDictionary<string, Transaction>();
        }

        public void AddPending(Transaction transaction)
        {
            _pending.TryAdd(transaction.Hash, transaction);
        }

        public void RemovePedning(IEnumerable<Transaction> minnedTrans)
        {
            foreach (var minned in minnedTrans) _pending.TryRemove(minned.Hash, out _);
        }

        public IEnumerable<Transaction> GetPending()
        {
            return _pending.Values;
        }
    }
}