using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.Models;

namespace Node.CLI.Handlers
{
    public class ChainHandler : INotificationHandler<ChainViewModel>
    {
        public ChainHandler()
        {
        }

        public Task Handle(ChainViewModel newChain, CancellationToken cancellationToken)
        {
            // clear chache
            throw new NotImplementedException();
        }
    }
}