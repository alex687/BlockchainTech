using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Node.CLI.Models;
using Node.CLI.Repositories;
using Node.CLI.Services;
using Node.Core.Models;

namespace Node.CLI.Handlers
{
    public class BlockHandler : INotificationHandler<BlockViewModel>
    {
        private readonly IMapper _mapper;
        private readonly PeerService _peerService;
        private readonly TransactionCache _tranCache;

        public BlockHandler(TransactionCache tranCache, IMapper mapper, PeerService peerService)
        {
            _tranCache = tranCache;
            _mapper = mapper;
            _peerService = peerService;
        }

        public async Task Handle(BlockViewModel newBlockAdded, CancellationToken cancellationToken)
        {
            var minedTransactions = newBlockAdded.Transactions
                .Select(_mapper.Map<TransactionViewModel, Transaction>);
            await _peerService.NotifyAll(newBlockAdded);

            _tranCache.AddTransactions(minedTransactions);
        }
    }
}