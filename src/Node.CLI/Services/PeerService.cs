using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Node.CLI.Models;
using Node.Core.Models;

namespace Node.CLI.Services
{
    public class PeerService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IList<Peer> _peers;

        public PeerService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
            _peers = new List<Peer>();
        }

        public async Task Add(Peer peer)
        {
            if (_peers.All(p => p.Address != peer.Address))
            {
                _peers.Add(peer);
                var peerModel = _mapper.Map<Peer, PeerViewModel>(peer);
                await _mediator.Publish(peerModel);
            }
        }

        public IEnumerable<Peer> GetAll()
        {
            return _peers.ToImmutableList();
        }
    }
}