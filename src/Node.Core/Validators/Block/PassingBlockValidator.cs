using Node.Core.Validators.Transactions;

namespace Node.Core.Validators.Block
{
    public class PassingBlockValidator : IBlockValidator
    {
        private readonly ITransactionValidator _transactionValidator;

        public PassingBlockValidator(ITransactionValidator transactionValidator)
        {
            _transactionValidator = transactionValidator;
        }

        public bool Validate(Models.Block block, Models.Block previousBlock)
        {
            return _transactionValidator.MinedTransactionsValidate(block.Transactions, block.Index);
        }
    }
}