using Node.Core.Models;

namespace Node.Core.Validators.Transactions
{
    public class PassingTranValidator : ITransactionValidator
    {
        public bool Validate(Transaction transaction)
        {
            return true;
        }
    }
}