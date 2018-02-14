using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.Models;
using Node.CLI.Repositories.Caches;
using Node.CLI.Services;

namespace Node.CLI.Handlers
{
    public class AddedBlockToChainHandler : INotificationHandler<BlockNotify>
    {
        private readonly CommunicationService _communicationService;
        private readonly TransactionCache _tranCache;

        public AddedBlockToChainHandler(TransactionCache tranCache, CommunicationService communicationService)
        {
            _tranCache = tranCache;
            _communicationService = communicationService;
        }

        public async Task Handle(BlockNotify newBlock, CancellationToken cancellationToken)
        {
            await _communicationService.PublishBlock(newBlock);

            var minedTransactions = newBlock.Block.Transactions;
            _tranCache.AddTransaction(minedTransactions);
        }
    }
}