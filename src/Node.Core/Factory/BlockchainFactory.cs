using Node.Core.Repositories.Blockchain;
using Node.Core.Validators.Block;
using Node.Core.Validators.Transactions;

namespace Node.Core.Factory
{
    public class BlockchainFactory : IBlockchainFactory
    {
        public BlockRepository CreateBlockchain()
        {
            var transactionsRepository = new TransactionsRepository();
            var pendingTransaction = new PendingTransactionRepository();

            var transactionValidator = new TransactionValidator(transactionsRepository);
            var blockValidator = new PassingBlockValidator(transactionValidator);

            var blockRepository = new BlockRepository(transactionsRepository, blockValidator, pendingTransaction, transactionValidator);

            return blockRepository;
        }
    }
}
