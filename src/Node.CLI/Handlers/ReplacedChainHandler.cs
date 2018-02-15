using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.Models;
using Node.CLI.Services;

namespace Node.CLI.Handlers
{
    public class ReplacedChainHandler : INotificationHandler<ChainNotify>
    {
        private readonly CommunicationService _communicationService;

        public ReplacedChainHandler(CommunicationService communicationService)
        {
            _communicationService = communicationService;
        }

        public async Task Handle(ChainNotify newChain, CancellationToken cancellationToken)
        {
            await _communicationService.PublishChain(newChain);
        }
    }
}