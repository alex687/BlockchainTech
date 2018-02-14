using System.Collections.Generic;
using Node.Core.Models;

namespace Node.Core.Validators.Transactions
{
    public interface ITransactionValidator
    {
        bool Validate(IEnumerable<Transaction> transactions);

        bool Validate(Transaction transaction);
    }
}