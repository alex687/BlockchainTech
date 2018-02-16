using MediatR;
using Node.Core.Models;

namespace Node.CLI.InternalModels
{
    public class PeerNotify : INotification
    {
        public PeerNotify(Peer peer)
        {
            Peer = peer;
        }

        public Peer Peer { get; }
    }
}