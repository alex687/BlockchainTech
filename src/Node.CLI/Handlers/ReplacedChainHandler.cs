using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.Models;
using Node.CLI.Repositories.Caches;
using Node.CLI.Services;

namespace Node.CLI.Handlers
{
    public class ReplacedChainHandler : INotificationHandler<ReplacedChainNotify>
    {
        private readonly PeerService _peerService;
        private readonly TransactionCache _tranCache;

        public ReplacedChainHandler(TransactionCache tranCache, PeerService peerService)
        {
            _tranCache = tranCache;
            _peerService = peerService;
        }

        public async Task Handle(ReplacedChainNotify newChain, CancellationToken cancellationToken)
        {
            _tranCache.ReloadCache(newChain.Blocks);
            await _peerService.NotifyAll(newChain);
        }
    }
}