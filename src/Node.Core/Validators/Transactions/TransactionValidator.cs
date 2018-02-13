using Node.Core.Utils;
using Node.Core.Models;

namespace Node.Core.Validators.Transactions
{
    public class TransactionValidator : ITransactionValidator
    {
        public bool ValidateTransaction(Transaction transaction)
        {
            if (!IsValidAddress(transaction))
            {
                return false;
            }

            if (!IsValidSignature(transaction))
            {
                return false;
            }

            return true;
        }

        private bool IsValidSignature(Transaction transaction)
        {
            //TODO validate signature
            return true;
        }

        private bool IsValidAddress(Transaction transaction)
        {
            return transaction.From == transaction.SenderPublickKey.ComputeRipeMd160Hash();
        }
    }
}
