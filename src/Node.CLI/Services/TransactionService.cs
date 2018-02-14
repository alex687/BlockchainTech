using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Node.CLI.Models;
using Node.CLI.Repositories;
using Node.Core.Models;
using Node.Core.Validators.Transactions;

namespace Node.CLI.Services
{
    public class TransactionService
    {
        private readonly TransactionRepository _tranRepo;
        private readonly ITransactionValidator _tranValidator;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly BlockRepository _blockRepo;

        public TransactionService(BlockRepository blockRepo, TransactionRepository tranRepo, ITransactionValidator tranValidator, IMediator mediator, IMapper mapper)
        {
            _blockRepo = blockRepo;
            _tranRepo = tranRepo;
            _tranValidator = tranValidator;
            _mediator = mediator;
            _mapper = mapper;
        }

        public Transaction GetTransaction(string hash)
        {
            return GetConfirmedTransactions(0)
                .FirstOrDefault(t => t.Hash == hash);
        }

        public decimal GetBalance(string address, int confirmations)
        {
            var accountTransactions = GetConfirmedTransactions(confirmations);

            var from = accountTransactions
                .Where(t => t.From == address)
                .Sum(t => t.Amount);

            var to = accountTransactions
                .Where(t => t.To == address)
                .Sum(t => t.Amount);

            return to - from;
        }

        public async Task AddPendingTransaction(TransactionViewModel transaction)
        {
            var domainObject = _mapper.Map<TransactionViewModel, Transaction>(transaction);

            if (_tranValidator.ValidateTransaction(domainObject))
            {
                _tranRepo.AddPending(domainObject);

                await _mediator.Publish(transaction);
            }
        }

        private IEnumerable<Transaction> GetConfirmedTransactions(int confirmations)
        {
            var blocks = _blockRepo.GetBlocks();
            var toTake = blocks.Count() - confirmations;
            return blocks.Take(toTake).SelectMany(b => b.Transactions);
        }
    }
}