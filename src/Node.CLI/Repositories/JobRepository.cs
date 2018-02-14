using System.Collections.Concurrent;
using Node.Core.Models;

namespace Node.CLI.Repositories
{
    public class JobRepository
    {
        // address -> job
        private readonly ConcurrentDictionary<string, Block> _jobs;

        public JobRepository()
        {
            _jobs = new ConcurrentDictionary<string, Block>();
        }
    }
}