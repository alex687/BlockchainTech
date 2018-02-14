using System.Collections.Concurrent;
using Node.Core.Models;

namespace Node.Core.Repositories
{
    public class MiningJobsRepository
    {
        private readonly ConcurrentDictionary<string, Block> _jobs;

        public MiningJobsRepository()
        {
            _jobs = new ConcurrentDictionary<string, Block>();
        }

        public Block GetJob(string address)
        {
            _jobs.TryGetValue(address, out var block);
            return block;
        }

        public void PutMinnerJob(string address, Block block)
        {
            _jobs.AddOrUpdate(address, block, (a, b) => block);
        }
    }
}