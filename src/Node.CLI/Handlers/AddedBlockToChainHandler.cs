using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.Models;
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
<<<<<<< HEAD
            await _communicationService.NotifyAll(newBlockAdded);
=======
            await _communicationService.PublishBlock(newBlock);

            var minedTransactions = newBlock.Block.Transactions;
            _tranCache.AddTransaction(minedTransactions);
>>>>>>> cbe036fcf2c079e34d1e78d57a9361f76c6538a9
        }
    }
}