using System;
using System.Threading;
using System.Threading.Tasks;
using Node.Core.Models;

namespace Minner
{
    public class Manager
    {
        private readonly Logger _logger;
        private readonly NodeCommunicator _nodeCommunicatorService;
        private CancellationTokenSource _cancelationSource;
        private Block _currentBlock;

        public Manager(Logger logger, NodeCommunicator nodeCommunicatorService)
        {
            _logger = logger;
            _nodeCommunicatorService = nodeCommunicatorService;
        }

        public async Task Start()
        {
            while (true)
            {
                try
                {
                    await Mine();
                    await Task.Delay(new TimeSpan(0, 0, 5), _cancelationSource.Token);
                }
                catch{}
            }
        }

        private async Task Mine()
        {
            var block = await _nodeCommunicatorService.GetBlockToMine();
            if (_currentBlock == null || _currentBlock.Index < block.Index)
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

            Task.Run(() => Compute(), _cancelationSource.Token)
                .ContinueWith(async block =>
                {
                    _cancelationSource.Cancel();
                    await _nodeCommunicatorService.SendBlock(await block);
                });
        }

        private Block Compute()
        {
            var worker = new Worker(_currentBlock, long.MinValue, long.MaxValue);
            return worker.Compute();
        }
    }
}