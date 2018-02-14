using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Node.CLI.Controllers;
using Node.CLI.Models;
using Node.CLI.Services;
using Node.Core.Models;

namespace Node.CLI.Handlers
{
    public class PeerSyncHandler : INotificationHandler<PeerViewModel>
    {
        private readonly BlockService _blockService;
        private readonly IUrlHelper _urlHelper;

        public PeerSyncHandler(BlockService blockService, IUrlHelper urlHelper)
        {
            _blockService = blockService;
            _urlHelper = urlHelper;
        }

        public async Task Handle(PeerViewModel newPeer, CancellationToken cancellationToken)
        {
            var url = _urlHelper.Action(nameof(BlockController.GetBlocks), nameof(BlockController));
            // get all blocks of new peer
            var newPeerBlocks = new List<Block>();

            await _blockService.SyncBlocks(newPeerBlocks);

            throw new NotImplementedException();
        }
    }
}