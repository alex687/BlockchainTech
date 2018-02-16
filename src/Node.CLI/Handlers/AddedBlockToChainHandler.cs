using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.InternalModels;
using Node.CLI.Services;

namespace Node.CLI.Handlers
{
    public class AddedBlockToChainHandler : INotificationHandler<BlockNotify>
    {
        private readonly CommunicationService _communicationService;

        public AddedBlockToChainHandler(CommunicationService communicationService)
        {
            _communicationService = communicationService;
        }

        public async Task Handle(BlockNotify newBlock, CancellationToken cancellationToken)
        {
            await _communicationService.PublishBlock(newBlock);
        }
    }
}