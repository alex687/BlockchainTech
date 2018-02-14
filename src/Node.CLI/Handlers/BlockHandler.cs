using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.Models;

namespace Node.CLI.Handlers
{
    public class BlockHandler : INotificationHandler<BlockViewModel>
    {
        public Task Handle(BlockViewModel newBlockAdded, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}