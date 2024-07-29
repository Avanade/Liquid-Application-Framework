using Liquid.Core.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Core.Interfaces
{
    /// <summary>
    /// Handles message consuming process.
    /// </summary>
    /// <typeparam name="TEntity">Type of message body.</typeparam>
    public interface ILiquidConsumer<TEntity>
    {
        /// <summary>
        /// Initialize handler for consume <typeparamref name="TEntity"/> messages from topic or queue.
        /// </summary>
        void RegisterMessageHandler(CancellationToken cancellationToken = default);

        /// <summary>
        /// Defining the message processing function.
        /// </summary>
        event Func<ConsumerMessageEventArgs<TEntity>, CancellationToken, Task> ConsumeMessageAsync;

        /// <summary>
        /// Definition of the error handling process.
        /// </summary>
        event Func<ConsumerErrorEventArgs, Task> ProcessErrorAsync;
    }
}
