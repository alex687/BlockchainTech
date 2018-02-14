using Node.Core.Models;

namespace Node.Core.Validators.Transactions
{
    public interface ITransactionValidator
    {
        bool Validate(Transaction transaction);
    }
}