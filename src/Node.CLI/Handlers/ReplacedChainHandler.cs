using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.Models;
using Node.CLI.Repositories.Caches;
using Node.CLI.Services;

namespace Node.CLI.Handlers
{
    public class ReplacedChainHandler : INotificationHandler<ChainNotify>
    {
        private readonly CommunicationService _communicationService;
        private readonly TransactionCache _tranCache;

        public ReplacedChainHandler(TransactionCache tranCache, CommunicationService communicationService)
        {
            _tranCache = tranCache;
            _communicationService = communicationService;
        }

        public async Task Handle(ChainNotify newChain, CancellationToken cancellationToken)
        {
            _tranCache.ReloadCache(newChain.Blocks);
            await _communicationService.PublishChain(newChain);
        }
    }
}