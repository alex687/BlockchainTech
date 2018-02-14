using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Node.CLI.Models;
using Node.CLI.Repositories;
using Node.Core.Models;

namespace Node.CLI.Handlers
{
    public class BlockHandler : INotificationHandler<BlockViewModel>
    {
        private readonly IMapper _mapper;
        private readonly TransactionCache _tranCache;

        public BlockHandler(TransactionCache tranCache, IMapper mapper)
        {
            _tranCache = tranCache;
            _mapper = mapper;
        }

        public Task Handle(BlockViewModel newBlockAdded, CancellationToken cancellationToken)
        {
            var minedTransactions = newBlockAdded.Transactions
                .Select(_mapper.Map<TransactionViewModel, Transaction>);

            _tranCache.AddTransactions(minedTransactions);

            return Task.CompletedTask;
        }
    }
}