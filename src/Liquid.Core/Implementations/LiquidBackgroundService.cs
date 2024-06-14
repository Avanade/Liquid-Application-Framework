using Liquid.Core.Entities;
using Liquid.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Core.Implementations
{
    /// <summary>
    /// Liquid BackgroundService implementation for message consumers.
    /// </summary>
    /// <typeparam name="TEntity">Type of message body.</typeparam>
    public class LiquidBackgroundService<TEntity> : BackgroundService
    {
        private readonly ILiquidConsumer<TEntity> _consumer;

        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initialize a new instance of <see cref="LiquidBackgroundService{TEntity}"/>
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="consumer">Consumer service with message handler definition for processing messages.</param>
        public LiquidBackgroundService(IServiceProvider serviceProvider, ILiquidConsumer<TEntity> consumer)
        {
            _serviceProvider = serviceProvider;
            _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
        }

        /// <summary>
        /// This method is called when the Microsoft.Extensions.Hosting.IHostedService starts.
        /// Its return a task that represents the lifetime of the long 
        /// running operation(s) being performed.
        /// </summary>
        /// <param name="stoppingToken">Triggered when Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)  
        /// is called.</param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.ConsumeMessageAsync += ProcessMessageAsync;
            _consumer.RegisterMessageHandler();

            await Task.CompletedTask;
        }

        /// <summary>
        /// This method is called when message handler gets a message.
        /// Return a task that represents the process to be executed 
        /// by the message handler.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        public async Task ProcessMessageAsync(ConsumerMessageEventArgs<TEntity> args, CancellationToken cancellationToken)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var worker = scope.ServiceProvider.GetRequiredService<ILiquidWorker<TEntity>>();
                await worker.ProcessMessageAsync(args, cancellationToken);
            }
        }
    }
}
