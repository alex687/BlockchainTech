using System.Linq;
using Node.Core.Crypto;
using Node.Core.Models;

namespace Node.Core.Utils
{
    public static class BlockUtils
    {
        public static string ComputeHash(this Block block)
        {
            var transactionsHash = string.Join(string.Empty, block.Transactions.Select(t => t.ComputeHash()));

            return Hash.ComputeHash(block.Index.ToString(), block.PreviousBlockHash, block.CreatedOn.ToString("O"),
                transactionsHash, block.Difficulty.ToString(), block.Nonce.ToString());
        }
    }
}
