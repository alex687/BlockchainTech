using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Node.CLI.Models;
using Node.CLI.Repositories.Caches;
using Node.CLI.Services;
using Node.Core.Models;

namespace Node.CLI.Handlers
{
    public class BlockHandler : INotificationHandler<BlockViewModel>
    {
        private readonly IMapper _mapper;
        private readonly CommunicationService _communicationService;
        private readonly TransactionCache _tranCache;

        public BlockHandler(TransactionCache tranCache, IMapper mapper, CommunicationService communicationService)
        {
            _tranCache = tranCache;
            _mapper = mapper;
            _communicationService = communicationService;
        }

        public async Task Handle(BlockViewModel newBlockAdded, CancellationToken cancellationToken)
        {
            var minedTransactions = newBlockAdded.Transactions
                .Select(_mapper.Map<TransactionViewModel, Transaction>);

            await _communicationService.NotifyAll(newBlockAdded);

            _tranCache.AddTransaction(minedTransactions);
        }
    }
}