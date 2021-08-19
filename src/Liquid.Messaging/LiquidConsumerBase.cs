using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging
{
    public abstract class LiquidConsumerBase<TEntity> : BackgroundService
    {
        private readonly ILiquidConsumer<TEntity> _consumer;

        public LiquidConsumerBase(ILiquidConsumer<TEntity> consumer)
        {
            _consumer = consumer;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.ProcessMessageAsync += ProcessMessageAsync;

            _consumer.Start();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        public abstract Task ProcessMessageAsync(ProcessMessageEventArgs<TEntity> args, CancellationToken cancellationToken);
    }
}
