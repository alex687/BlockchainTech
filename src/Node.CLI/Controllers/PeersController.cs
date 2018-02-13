using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Node.CLI.Services;
using Node.Core.Models;

namespace Node.CLI.Controllers
{
    [Route("api/[controller]")]
    public class PeersController
    {
        private readonly PeerService _peerService;

        public PeersController(PeerService peerService)
        {
            _peerService = peerService;
        }

        [HttpGet]
        public IEnumerable<Peer> GetPeers()
        {
            return _peerService.GetAll();
        }

        [HttpPost]
        public async Task AddPeer([FromBody] Peer peer)
        {
            await _peerService.Add(peer);
        }
    }
}