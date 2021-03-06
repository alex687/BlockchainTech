﻿using System.Collections.Generic;
using Node.Core.Models;

namespace Node.Core.Validators.Transactions
{
    public class PassingTranValidator : ITransactionValidator
    {
        public bool PendingTransactionsValidate(IEnumerable<Transaction> transactions)
        {
            return true;
        }

        public bool PendingTransactionsValidate(IEnumerable<PendingTransaction> transactions)
        {
            return true;
        }

        public bool MinedTransactionsValidate(IEnumerable<Transaction> transactions, int blockIndex)
        {
            return true;
        }

        public bool Validate(PendingTransaction transaction)
        {
            return true;
        }

        public bool Validate(Transaction transaction)
        {
            return true;
        }
    }
}