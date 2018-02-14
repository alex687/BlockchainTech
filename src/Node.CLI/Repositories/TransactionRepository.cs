using System.Collections.Generic;
using System.Linq;
using Node.Core;
using Node.Core.Models;

namespace Node.CLI.Repositories
{
    public class TransactionRepository
    {
        private static readonly List<Transaction> Pending = new List<Transaction>();
        private readonly IEnumerable<Block> _blocks;

        public TransactionRepository(IBlockchain blockchain)
        {
            _blocks = blockchain.GetBlocks();
        }

        public IEnumerable<Transaction> GetTransactions()
        {
            return GetConfirmedTransactions(0);
        }

        public IEnumerable<Transaction> GetConfirmedTransactions(int confirmations)
        {
            var toTake = _blocks.Count() - confirmations;
            return _blocks.Take(toTake).SelectMany(b => b.Transactions);
        }

        public void AddPending(Transaction transaction)
        {
            Pending.Add(transaction);
        }

        public void RemovePedning(IEnumerable<Transaction> minnedTrans)
        {
            Pending.RemoveAll(t => minnedTrans.Any(minned => t.Hash == minned.Hash));
        }

        public IEnumerable<Transaction> GetPending()
        {
            return Pending.AsReadOnly();
        }
    }
}