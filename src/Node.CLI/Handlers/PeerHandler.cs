using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.Models;

namespace Node.CLI.Handlers
{
    public class PeerSyncHandler : INotificationHandler<PeerViewModel>
    {
        public PeerSyncHandler()
        {
        }

        public Task Handle(PeerViewModel newPeer, CancellationToken cancellationToken)
        {
            // get all blocks of new peer
            // sync with our block chain
            throw new NotImplementedException();
        }
    }
}