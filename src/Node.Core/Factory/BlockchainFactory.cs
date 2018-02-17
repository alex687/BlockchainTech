using Node.Core.Repositories.Blockchain;
using Node.Core.Validators.Block;
using Node.Core.Validators.Transactions;

namespace Node.Core.Factory
{
    public class BlockchainFactory : IBlockchainFactory
    {
        public Blockchain CreateBlockchain()
        {
            var transactionsRepository = new TransactionsRepository();
            var pendingTransactionRepository = new PendingTransactionRepository();

            var transactionValidator = new TransactionValidator(transactionsRepository);
            var blockValidator = new BlockValidator(transactionValidator);

            var blockRepository = new Blockchain(transactionsRepository, pendingTransactionRepository, blockValidator, transactionValidator);

            return blockRepository;
        }
    }
}
