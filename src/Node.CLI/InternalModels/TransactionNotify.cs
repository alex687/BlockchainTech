using MediatR;
using Node.Core.Models;

namespace Node.CLI.InternalModels
{
    public class TransactionNotify : INotification
    {
        public TransactionNotify(PendingTransaction transaction)
        {
            Transaction = transaction;
        }

        public PendingTransaction Transaction { get; }
    }
}