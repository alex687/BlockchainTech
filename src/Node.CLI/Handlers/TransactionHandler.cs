using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.Models;
using Node.CLI.Repositories;

namespace Node.CLI.Handlers
{
    public class TransactionHandler : INotificationHandler<TransactionNotify>
    {
        private readonly BlockRepository _blockRepo;
        private readonly PendingTransactionRepository _tranRepo;

        public TransactionHandler(PendingTransactionRepository tranRepo, BlockRepository blockRepo)
        {
            _tranRepo = tranRepo;
            _blockRepo = blockRepo;
        }

        public Task Handle(TransactionNotify notify, CancellationToken cancellationToken)
        {
            var transaction = notify.Transaction;
            return Task.CompletedTask;
        }
    }
}