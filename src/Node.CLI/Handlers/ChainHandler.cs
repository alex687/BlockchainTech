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
    public class ChainHandler : INotificationHandler<ChainViewModel>
    {
        private readonly IMapper _mapper;
        private readonly PeerService _peerService;
        private readonly TransactionCache _tranCache;

        public ChainHandler(TransactionCache tranCache, IMapper mapper, PeerService peerService)
        {
            _tranCache = tranCache;
            _mapper = mapper;
            _peerService = peerService;
        }

        public async Task Handle(ChainViewModel newChain, CancellationToken cancellationToken)
        {
            var domainBlocks = newChain.Blocks.Select(_mapper.Map<BlockViewModel, Block>);
            _tranCache.UpdateBlocks(domainBlocks);

            await _peerService.NotifyAll(newChain);
        }
    }
}