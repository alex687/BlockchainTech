using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.Models;
using Node.Core.Models;
using Node.Core.Repositories;

namespace Node.CLI.Services
{
    public class PeerService
    {
        private readonly IMediator _mediator;
        private readonly PeerRepository _peers;

        public PeerService(IMediator mediator, PeerRepository peers)
        {
            _mediator = mediator;
            _peers = peers;
        }

        public async Task Add(Peer peer)
        {
            var existing = _peers.Get(peer.Address);
            if (existing == null)
            {
                _peers.Add(peer);
                await _mediator.Publish( new PeerNotify(peer));
            }
        }

        public IEnumerable<Peer> All()
        {
            return _peers.GetAll();
        }
    }
}