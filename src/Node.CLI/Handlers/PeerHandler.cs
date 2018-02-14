using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Node.CLI.Controllers;
using Node.CLI.Models;
using Node.CLI.Services;

namespace Node.CLI.Handlers
{
    public class PeerSyncHandler : INotificationHandler<PeerViewModel>
    {
        private readonly BlockService _blockService;
        private readonly PeerService _peerService;

        public PeerSyncHandler(BlockService blockService, PeerService peerService)
        {
            _blockService = blockService;
            _peerService = peerService;
        }

        public async Task Handle(PeerViewModel newPeer, CancellationToken cancellationToken)
        {
            var newPeerBlocks = await _peerService.GetPeerBlocks(newPeer.Address);
            await _blockService.SyncBlocks(newPeerBlocks);
        }
    }
}