using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.InternalModels;
using Node.Core.Models;

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
            return _blockchainInstance.Blockchain.GetTransaction(hash);
        }

        public IEnumerable<PendingTransaction> GetPendingTransactions()
        {
            return _blockchainInstance.Blockchain.GetPending();
        }

        public decimal GetBalance(string address, int confirmations)
        {
            return _blockchainInstance.Blockchain.GetBalance(address, confirmations);
        }

        public async Task<bool> AddPendingTransaction(PendingTransaction transaction)
        {
            if (_blockchainInstance.Blockchain.AddPending(transaction))
            {
                await _mediator.Publish(new TransactionNotify(transaction));
                return true;
            }

            return false;
        }
    }
}