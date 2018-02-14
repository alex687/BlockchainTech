﻿using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Node.CLI.Models;
using Node.Core.Caches;
using Node.Core.Models;
using Node.Core.Repositories;
using Node.Core.Validators.Transactions;

namespace Node.CLI.Services
{
    public class TransactionService
    {
        private readonly PendingTransactionRepository _tranRepo;
        private readonly ITransactionValidator _tranValidator;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly TransactionCache _transactionsCache;

        public TransactionService(TransactionCache transactionsCache, PendingTransactionRepository tranRepo, ITransactionValidator tranValidator, IMediator mediator, IMapper mapper)
        {
            _transactionsCache = transactionsCache;
            _tranRepo = tranRepo;
            _tranValidator = tranValidator;
            _mediator = mediator;
            _mapper = mapper;
        }

        public Transaction GetTransaction(string hash)
        {
            return _transactionsCache.GetTransaction(hash);
        }

        public decimal GetBalance(string address, int confirmations)
        {
            var addressTransactions = _transactionsCache.GeTransactions(address);

            var from = addressTransactions
                .Where(t => t.From == address)
                .Sum(t => t.Amount);

            var to = addressTransactions
                .Where(t => t.To == address)
                .Sum(t => t.Amount);

            return to - from;
        }

        public async Task AddPendingTransaction(TransactionViewModel transaction)
        {
            var domainObject = _mapper.Map<TransactionViewModel, Transaction>(transaction);

            if (_tranValidator.Validate(domainObject))
            {
                _tranRepo.AddPending(domainObject);

                await _mediator.Publish(transaction);
            }
        }
    }
}