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
    public class ChainHandler : INotificationHandler<ChainViewModel>
    {
        private readonly CommunicationService _communicationService;
        private readonly IMapper _mapper;
        private readonly TransactionCache _tranCache;

        public ChainHandler(TransactionCache tranCache, IMapper mapper, CommunicationService communicationService)
        {
            _communicationService = communicationService;
            _tranCache = tranCache;
            _mapper = mapper;
        }

        public async Task Handle(ChainViewModel newChain, CancellationToken cancellationToken)
        {
            var domainBlocks = newChain.Blocks.Select(_mapper.Map<BlockViewModel, Block>);
            _tranCache.ReloadCache(domainBlocks);

            await _communicationService.NotifyAll(newChain);
        }
    }
}