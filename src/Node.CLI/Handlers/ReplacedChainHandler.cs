using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.Models;
using Node.CLI.Services;
using Node.Core.Caches;

namespace Node.CLI.Handlers
{
    public class ReplacedChainHandler : INotificationHandler<ReplacedChainNotify>
    {
        private readonly CommunicationService _communicationService;

        public ReplacedChainHandler(CommunicationService communicationService)
        {
            _communicationService = communicationService;
        }

        public async Task Handle(ReplacedChainNotify newChain, CancellationToken cancellationToken)
        {
            await _communicationService.NotifyAll(newChain);
        }
    }
}