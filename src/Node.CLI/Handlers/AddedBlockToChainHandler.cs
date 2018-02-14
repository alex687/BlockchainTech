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
        private readonly PeerService _peerService;
        private readonly TransactionCache _tranCache;

        public AddedBlockToChainHandler(TransactionCache tranCache, PeerService peerService)
        {
            _tranCache = tranCache;
            _peerService = peerService;
        }

        public async Task Handle(AddedBlockToChainNotify newBlockAdded, CancellationToken cancellationToken)
        {
            await _peerService.NotifyAll(newBlockAdded);

            var minedTransactions = newBlockAdded.Block.Transactions;
            _tranCache.AddTransaction(minedTransactions);
        }
    }
}