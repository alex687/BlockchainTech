using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Node.CLI.Models;
using Node.CLI.Repositories;

namespace Node.CLI.Handlers
{
    public class TransactionHandler : INotificationHandler<TransactionViewModel>
    {
        private readonly IMapper _mapper;
        private readonly TransactionRepository _tranRepo;

        public TransactionHandler(IMapper mapper, TransactionRepository tranRepo)
        {
            _mapper = mapper;
            _tranRepo = tranRepo;
        }

        public Task Handle(TransactionViewModel newTran, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}