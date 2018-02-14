using System.Collections.Concurrent;
using Node.Core.Models;

namespace Node.CLI.Repositories
{
    public class MiningJobsRepository
    {
        // address -> job
        private readonly ConcurrentDictionary<string, Block> _jobs;

        public MiningJobsRepository()
        {
            _jobs = new ConcurrentDictionary<string, Block>();
        }
    }
}