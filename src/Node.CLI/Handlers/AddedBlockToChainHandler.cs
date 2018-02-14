using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.Models;
using Node.CLI.Repositories.Caches;
using Node.CLI.Services;

namespace Node.CLI.Handlers
{
    public class AddedBlockToChainHandler : INotificationHandler<AddedBlockToChainNotify>
    {
        private readonly CommunicationService _communicationService;
        private readonly TransactionCache _tranCache;

        public AddedBlockToChainHandler(TransactionCache tranCache, CommunicationService communicationService)
        {
            _tranCache = tranCache;
            _communicationService = communicationService;
        }

        public async Task Handle(AddedBlockToChainNotify newBlockAdded, CancellationToken cancellationToken)
        {
            await _communicationService.NotifyAll(newBlockAdded);

            var minedTransactions = newBlockAdded.Block.Transactions;
            _tranCache.AddTransaction(minedTransactions);
        }
    }
}