using System.Collections.Generic;
using System.Linq;
using Node.Core.Models;

namespace Node.Core.Extensions
{
    public static class TransactionsExtensions
    {
        public static decimal CalculateReward(this IEnumerable<Transaction> transactions)
        {
            return 0.5M + transactions.Sum(transaction => transaction.Amount * 0.01M);
        }
    }
}