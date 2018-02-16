using System;
using System.Threading;
using System.Threading.Tasks;
using Node.Core.Models;

namespace Minner
{
    public class Manager
    {
        private readonly Communication _communicationService;
        private CancellationTokenSource _cancelationSource;
        private Block _currentBlock;

        public Manager(Communication communicationService)
        {
            _communicationService = communicationService;
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
            var block = await _communicationService.GetBlockToMine();
            if (_currentBlock == null || _currentBlock.Index < block.Index)
            {
                _currentBlock = block;

                _cancelationSource?.Cancel();
                await CreateNewWorker();
            }
        }

        private async Task CreateNewWorker()
        {
            _cancelationSource = new CancellationTokenSource();

            Task.Run(() => Compute(), _cancelationSource.Token)
                .ContinueWith(async block =>
                {
                    _cancelationSource.Cancel();
                    _communicationService.SendBlock(await block);
                });
        }

        private Block Compute()
        {
            return new Worker(_currentBlock, long.MinValue, long.MaxValue).Compute();
        }
    }
}