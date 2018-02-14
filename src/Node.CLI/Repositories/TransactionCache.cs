using System.Collections.Generic;
using System.Linq;
using Node.Core.Models;

namespace Node.CLI.Repositories
{
    public class TransactionCache
    {
        private readonly BlockRepository _blockRepo;

        public TransactionCache(BlockRepository blockRepo)
        {
            _blockRepo = blockRepo;
        }

        public IEnumerable<Transaction> GetConfirmedTransactions(int confirmations)
        {
            var blocks = _blockRepo.GetBlocks();
            var toTake = blocks.Count() - confirmations;
            return blocks.Take(toTake).SelectMany(b => b.Transactions);
        }

        public Transaction GetByHash(string hash)
        {
            return GetConfirmedTransactions(0)
                .FirstOrDefault(t => t.Hash == hash);
        }

        public void AddTransactions(IEnumerable<Transaction> minnedTransactions)
        {
        }

        public void UpdateBlocks(IEnumerable<Block> blocks)
        {
        }
    }
}