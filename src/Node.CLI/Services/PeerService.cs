using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Node.CLI.Models;
using Node.Core.Models;
using Node.Core.Repositories;

namespace Node.CLI.Services
{
    public class PeerService
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly PeerRepository _peers;

        public PeerService(IMediator mediator, IMapper mapper, PeerRepository peers)
        {
            _mediator = mediator;
            _mapper = mapper;
            _peers = peers;
        }

        public async Task Add(Peer peer)
        {
            var existing = _peers.Get(peer.Address);
            if (existing == null)
            {
                _peers.Add(peer);
                var peerModel = _mapper.Map<Peer, PeerViewModel>(peer);
                await _mediator.Publish(peerModel);
            }
        }

        public IEnumerable<Peer> All()
        {
            return _peers.GetAll();
        }
    }
}