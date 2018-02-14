using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.Models;
using Node.CLI.Services;

namespace Node.CLI.Handlers
{
    public class PeerSyncHandler : INotificationHandler<PeerViewModel>
    {
        private readonly BlockService _blockService;
        private readonly CommunicationService _communicationService;
        private readonly PeerService _peerService;

        public PeerSyncHandler(BlockService blockService, PeerService peerService,
            CommunicationService communicationService)
        {
            _blockService = blockService;
            _peerService = peerService;
            _communicationService = communicationService;
        }

        public async Task Handle(PeerViewModel newPeer, CancellationToken cancellationToken)
        {
            var newPeerBlocks = await _communicationService.GetPeerBlocks(newPeer.Address);

            var newPeerList = await _communicationService.GetPeerList(newPeer.Address);

            foreach (var peer in newPeerList) await _peerService.Add(peer);

            await _blockService.SyncBlocks(newPeerBlocks);
        }
    }
}