using System;
using System.Threading;
using System.Threading.Tasks;
using Node.Core.Models;

namespace Minner
{
    public class Manager
    {
        private readonly Logger _logger;
        private readonly BlockHandler _blockHandler;
        private CancellationTokenSource _cancelationSource;
        private Block _currentBlock;

        public Manager(Logger logger, BlockHandler blockHandler)
        {
            _logger = logger;
            _blockHandler = blockHandler;
        }

        public async Task Start()
        {
            while (true)
            {
                try
                {
                    await Mine();
                    await Task.Delay(new TimeSpan(0, 0, 5));
                }
                catch(Exception e )
                {
                }
            }
        }

        private async Task Mine()
        {
            var block = await _blockHandler.GetBlockToMine();
            if (block != null && (_currentBlock == null || _currentBlock.Index < block.Index))
            {
                _currentBlock = block;
                _cancelationSource?.Cancel();
                CreateNewWorker();
            }
        }

        private void CreateNewWorker()
        {
            _logger.Log($"Creating new minning job for block: {_currentBlock.Index}");
            _cancelationSource = new CancellationTokenSource();

            Task.Run(() => Compute(_cancelationSource.Token))
                .ContinueWith(async block =>
                {
                    _cancelationSource.Cancel();
                    _logger.Log($"Job {(await block).Index} minned!");
                    await _blockHandler.SendMinnedBlock(await block);
                });
        }

        private Block Compute(CancellationToken cancellationToken)
        {
            var worker = new Worker(_currentBlock, long.MinValue, long.MaxValue);
            return worker.Compute(cancellationToken);
        }
    }
}