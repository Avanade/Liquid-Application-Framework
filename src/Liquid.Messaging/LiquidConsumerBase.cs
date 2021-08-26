using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging
{
    /// <summary>
    /// Base worker service consumer implementation.
    /// </summary>
    /// <typeparam name="TEntity">Type of message body.</typeparam>
    public abstract class LiquidConsumerBase<TEntity> : BackgroundService
    {
        private readonly ILiquidConsumer<TEntity> _consumer;

        /// <summary>
        /// Initialize a new instance of <see cref="LiquidConsumerBase{TEntity}"/>
        /// </summary>
        /// <param name="consumer">Consumer service with message handler definition for processing messages.</param>
        public LiquidConsumerBase(ILiquidConsumer<TEntity> consumer)
        {
            _consumer = consumer;
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
            _consumer.ProcessMessageAsync += ProcessMessageAsync;

            _consumer.RegisterMessageHandler();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        /// <summary>
        /// This method is called when message handler gets a message.
        /// The implementation should return a task that represents 
        /// the process to be executed by the message handler.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task ProcessMessageAsync(ProcessMessageEventArgs<TEntity> args, CancellationToken cancellationToken);
    }
}
