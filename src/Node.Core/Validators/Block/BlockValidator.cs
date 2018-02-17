using System.Linq;
using Node.Core.Extensions;
using Node.Core.Validators.Transactions;

namespace Node.Core.Validators.Block
{
    public class BlockValidator : IBlockValidator
    {
        private static int _difficulty = 5;
        private readonly ITransactionValidator _transactionValidator;

        public BlockValidator(ITransactionValidator transactionValidator)
        {
            _transactionValidator = transactionValidator;
        }

        public bool Validate(Models.Block block, Models.Block previousBlock)
        {
            if (previousBlock.Index + 1 != block.Index)
            {
                return false;
            }

            if (previousBlock.Hash != block.PreviousBlockHash)
            {
                return false;
            }

            if (previousBlock.CreatedOn > block.CreatedOn)
            {
                return false;
            }

            if (!this.HasValidHash(block))
            {
                return false;
            }

            if (!ContainsOnlyValidTransactions(block))
            {
                return false;
            }

            return true;
        }

        private bool HasValidHash(Models.Block block)
        {
            if (block.Hash != block.ComputeHash())
            {
                return false;
            }

            if (block.Hash.Take(_difficulty).Any(t => t != '0'))
            {
                return false;
            }

            return true;
        }

        private bool ContainsOnlyValidTransactions(Models.Block block)
        {
            return _transactionValidator.MinedTransactionsValidate(block.Transactions, block.Index);
        }
    }
}
