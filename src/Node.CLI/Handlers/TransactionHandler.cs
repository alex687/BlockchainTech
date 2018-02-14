using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.Models;
using Node.Core.Repositories;
using Node.CLI.Repositories;
using Node.CLI.Services;

namespace Node.CLI.Handlers
{
    public class TransactionHandler : INotificationHandler<TransactionNotify>
    {
        private readonly BlockRepository _blockRepo;
        private readonly CommunicationService _communicationService;
        private readonly PendingTransactionRepository _tranRepo;

        public TransactionHandler(
            PendingTransactionRepository tranRepo,
            BlockRepository blockRepo,
            CommunicationService communicationService)
        {
            _tranRepo = tranRepo;
            _blockRepo = blockRepo;
            _communicationService = communicationService;
        }

        public async Task Handle(TransactionNotify notify, CancellationToken cancellationToken)
        {
            var transaction = notify.Transaction;
            await _communicationService.PublishTransaction(transaction);
        }
    }
}