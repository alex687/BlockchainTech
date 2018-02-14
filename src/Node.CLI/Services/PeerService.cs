﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Flurl;
using Flurl.Http;
using MediatR;
using Node.CLI.Models;
using Node.CLI.Repositories;
using Node.Core.Models;

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

        public async Task<IEnumerable<Block>> GetPeerBlocks(string address)
        {
            var url = address.AppendPathSegments("api", "block");
            return await url.GetJsonAsync<IEnumerable<Block>>();
        }

        public async Task NotifyAll(ReplacedChainNotify newChain)
        {
            foreach (var peer in _peers.GetAll())
                await peer.Address.AppendPathSegments("api", "block", "sync")
                    .PostJsonAsync(newChain.Blocks);
        }

        public async Task NotifyAll(AddedBlockToChainNotify newBlock)
        {
            foreach (var peer in _peers.GetAll())
                await peer.Address.AppendPathSegments("api", "block", "notify")
                    .PostJsonAsync(newBlock);
        }
    }
}