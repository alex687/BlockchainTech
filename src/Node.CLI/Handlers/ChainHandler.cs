using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Node.CLI.Models;
using Node.CLI.Repositories.Caches;
using Node.Core.Models;

namespace Node.CLI.Handlers
{
    public class ChainHandler : INotificationHandler<ChainViewModel>
    {
        private readonly IMapper _mapper;
        private readonly TransactionCache _tranCache;

        public ChainHandler(TransactionCache tranCache, IMapper mapper)
        {
            _tranCache = tranCache;
            _mapper = mapper;
        }

        public Task Handle(ChainViewModel newChain, CancellationToken cancellationToken)
        {
            var domainBlocks = newChain.Blocks.Select(_mapper.Map<BlockViewModel, Block>);
            _tranCache.ReloadCache(domainBlocks);

            return Task.CompletedTask;
        }
    }
}