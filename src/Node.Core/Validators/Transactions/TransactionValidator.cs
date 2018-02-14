using Node.Core.Utils;
using Node.Core.Models;

namespace Node.Core.Validators.Transactions
{
    public class TransactionValidator : ITransactionValidator
    {
        public bool Validate(Transaction transaction)
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
            return true;
        }

        private bool IsValidAddress(Transaction transaction)
        {
            return transaction.From == transaction.SenderPublickKey.ComputeRipeMd160Hash();
        }
    }
}
