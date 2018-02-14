using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Node.CLI.Models;
using Node.Core.Repositories;

namespace Node.CLI.Handlers
{
    public class TransactionHandler : INotificationHandler<TransactionViewModel>
    {
        private readonly BlockRepository _blockRepo;
        private readonly IMapper _mapper;
        private readonly PendingTransactionRepository _tranRepo;

        public TransactionHandler(IMapper mapper, PendingTransactionRepository tranRepo, BlockRepository blockRepo)
        {
            _mapper = mapper;
            _tranRepo = tranRepo;
            _blockRepo = blockRepo;
        }

        public Task Handle(TransactionViewModel newTran, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}