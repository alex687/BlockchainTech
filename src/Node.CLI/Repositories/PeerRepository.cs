using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Node.Core.Models;

namespace Node.CLI.Repositories
{
    public class PeerRepository
    {
        private readonly ConcurrentBag<Peer> _peers;

        public PeerRepository()
        {
            _peers = new ConcurrentBag<Peer>();
        }

        public IEnumerable<Peer> GetAll()
        {
            return _peers.ToImmutableList();
        }

        public void Add(Peer peer)
        {
            _peers.Add(peer);
        }

        public Peer Get(string address)
        {
            return _peers.FirstOrDefault(p => p.Address == address);
        }
    }
}