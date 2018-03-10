using System;
using System.Collections.Generic;
using System.Linq;
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
                catch (Exception)
                {
                }
            }
        }

        private async Task Mine()
        {
            var block = await _blockHandler.GetBlockToMine();
            if (block != null && (_currentBlock == null || _currentBlock.Index < block.Index))
            {
                _logger.Log("New block to mine " + block.Index);
                _currentBlock = block;
                _cancelationSource?.Cancel();

                CreateNewWorker();
            }
        }

        private async Task CreateNewWorker()
        {
            _logger.Log($"Creating new minning job for block: {_currentBlock.Index}");
            _cancelationSource = new CancellationTokenSource();

            var block = Compute(_cancelationSource.Token);
            if (block != null)
            {
                _logger.Log("Stopped old");
            }

            if (await _blockHandler.SendMinnedBlock(block))
            {
                _logger.Log($"Job {(block).Index} minned!");
            }
        }



        private Block Compute(CancellationToken cancellationToken)
        {
            var worker = new Worker(_currentBlock, long.MinValue, long.MaxValue);
            return worker.Compute(cancellationToken);
        }
    }
}