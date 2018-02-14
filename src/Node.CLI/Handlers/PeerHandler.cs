using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.Models;
using Node.CLI.Services;
using Node.Core.Models;

namespace Node.CLI.Handlers
{
    public class PeerSyncHandler : INotificationHandler<PeerViewModel>
    {
        private readonly BlockService _blockService;

        public PeerSyncHandler(BlockService blockService)
        {
            _blockService = blockService;
        }

        public async Task Handle(PeerViewModel newPeer, CancellationToken cancellationToken)
        {
            // get all blocks of new peer
            var newPeerBlocks = new List<Block>();

            await _blockService.SyncBlocks(newPeerBlocks);

            throw new NotImplementedException();
        }
    }
}