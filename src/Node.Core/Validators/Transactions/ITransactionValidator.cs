using System.Collections.Generic;
using Node.Core.Models;

namespace Node.Core.Validators.Transactions
{
    public interface ITransactionValidator
    {
        bool PendingTransactionsValidate(IEnumerable<PendingTransaction> transactions);

        bool MinedTransactionsValidate(IEnumerable<Transaction> transactions, int blockIndex);

        bool Validate(PendingTransaction transaction);
    }
}