using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Node.CLI.Models;
using Node.CLI.Repositories;
using Node.Core.Models;

namespace Node.CLI.Handlers
{
    public class TransactionHandler : INotificationHandler<TransactionViewModel>
    {
        private readonly IMapper _mapper;
        private readonly TransactionRepository _tranRepo;
        private readonly BlockRepository _blockRepo;

        public TransactionHandler(IMapper mapper, TransactionRepository tranRepo, BlockRepository blockRepo)
        {
            _mapper = mapper;
            _tranRepo = tranRepo;
            _blockRepo = blockRepo;
        }

        public Task Handle(TransactionViewModel newTran, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

      
    }
}