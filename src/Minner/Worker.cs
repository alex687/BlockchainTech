using System.Threading;
using Node.Core.Extensions;
using Node.Core.Models;

namespace Minner
{
    public class Worker
    {
        private readonly Acceptance _acceptance;
        private readonly Block _block;
        private readonly long _leftRange;
        private readonly long _rightRange;

        public Worker(Block block, long leftRange, long rightRange)
        {
            _block = block;
            _leftRange = leftRange;
            _rightRange = rightRange;
            _acceptance = new Acceptance(block.Difficulty);
        }

        public Block Compute(CancellationToken cancellationToken)
        {
            for (var index = _leftRange; index < _rightRange; index++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                _block.Nonce = index;
                var hash = _block.ComputeHash();

                if (_acceptance.IsValid(hash))
                {
                    return CreateBlock(hash);
                }
            }

            return null;
        }

        private Block CreateBlock(string hash)
        {
            return new Block(_block.Index,
                _block.Transactions,
                _block.Difficulty,
                _block.PreviousBlockHash,
                _block.MinedBy,
                _block.Nonce,
                _block.CreatedOn,
                hash);
        }
    }
}