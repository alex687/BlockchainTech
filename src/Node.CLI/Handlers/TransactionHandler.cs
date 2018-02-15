using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.Models;
using Node.CLI.Services;

namespace Node.CLI.Handlers
{
    public class TransactionHandler : INotificationHandler<TransactionNotify>
    {
        private readonly CommunicationService _communicationService;

        public TransactionHandler(
            CommunicationService communicationService)
        {
            _communicationService = communicationService;
        }

        public async Task Handle(TransactionNotify notify, CancellationToken cancellationToken)
        {
            var transaction = notify.Transaction;
            await _communicationService.PublishTransaction(transaction);
        }
    }
}