using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.Configurations;
using Node.CLI.Models;
using Node.Core.Models;
using Node.Core.Repositories;
using Node.Core.Repositories.Blockchain;
using Node.Core.Validators.Transactions;

namespace Node.CLI.Services
{
    public class TransactionService
    {
        private readonly IMediator _mediator;
        private readonly BlockchainInstanceHolder _blockchainInstance;

        public TransactionService(IMediator mediator, BlockchainInstanceHolder blockchainInstance)
        {
            _mediator = mediator;
            _blockchainInstance = blockchainInstance;
        }

        public Transaction GetTransaction(string hash)
        {
            return _blockchainInstance.BlockRepository.GetTransaction(hash);
        }

        public decimal GetBalance(string address, int confirmations)
        {
            return _blockchainInstance.BlockRepository.GetBalance(address, confirmations);
        }

        public async Task<bool> AddPendingTransaction(Transaction transaction)
        {
            if (_blockchainInstance.BlockRepository.AddPending(transaction))
            {
                await _mediator.Publish(new TransactionNotify(transaction));

                return true;
            }

            return false;
        }
    }
}