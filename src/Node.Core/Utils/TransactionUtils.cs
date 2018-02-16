using Node.Core.Crypto;
using Node.Core.Models;

namespace Node.Core.Utils
{
    public static class TransactionUtils
    {
        public static bool IsValidSignature(this PendingTransaction transaction)
        {
            var dataForSign = transaction.From + transaction.To + transaction.Amount;
            return Crypto.Crypto.VerifySignature(transaction.SenderPublickKey, dataForSign, transaction.SenderSignature);
        }

        public static string ComputeHash(this PendingTransaction transaction)
        {
            return Hash.ComputeHash(transaction.From, transaction.To, transaction.SenderPublickKey,
                transaction.SenderSignature, transaction.Amount.ToString());
        }

    }
}
