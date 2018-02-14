using MediatR;
using Node.Core.Models;

namespace Node.CLI.Models
{
    public class TransactionNotify : INotification
    {
        public TransactionNotify(Transaction transaction)
        {
            Transaction = transaction;
        }

        public Transaction Transaction { get; set; }
    }
}