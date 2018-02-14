using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.Models;
using Node.CLI.Services;

namespace Node.CLI.Handlers
{
    public class AddedBlockToChainHandler : INotificationHandler<AddedBlockToChainNotify>
    {
        private readonly CommunicationService _communicationService;

        public AddedBlockToChainHandler(CommunicationService communicationService)
        {
            _communicationService = communicationService;
        }

        public async Task Handle(AddedBlockToChainNotify newBlockAdded, CancellationToken cancellationToken)
        {
            await _communicationService.NotifyAll(newBlockAdded);
        }
    }
}