using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.Models;
using Node.CLI.Repositories;
using Node.CLI.Repositories.Caches;
using Node.Core.Models;
using Node.Core.Validators.Transactions;

namespace Node.CLI.Services
{
    public class TransactionService
    {
        private readonly PendingTransactionRepository _tranRepo;
        private readonly ITransactionValidator _tranValidator;
        private readonly IMediator _mediator;
        private readonly TransactionCache _transactionsCache;

        public TransactionService(TransactionCache transactionsCache, PendingTransactionRepository tranRepo, ITransactionValidator tranValidator, IMediator mediator)
        {
            _transactionsCache = transactionsCache;
            _tranRepo = tranRepo;
            _tranValidator = tranValidator;
            _mediator = mediator;
        }

        public Transaction GetTransaction(string hash)
        {
            return _transactionsCache.GetTransaction(hash);
        }

        public decimal GetBalance(string address, int confirmations)
        {
            var addressTransactions = _transactionsCache.GeTransactions(address, confirmations);

            var from = addressTransactions
                .Where(t => t.From == address)
                .Sum(t => t.Amount);

            var to = addressTransactions
                .Where(t => t.To == address)
                .Sum(t => t.Amount);

            return to - from;
        }

        public async Task AddPendingTransaction(Transaction transaction)
        {
            if (_tranValidator.Validate(transaction))
            {
                _tranRepo.AddPending(transaction);

                await _mediator.Publish(new TransactionNotify(transaction));
            }
        }
    }
}