using System;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.Interfaces
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
        void RegisterMessageHandler();

        /// <summary>
        /// Defining the message processing function.
        /// </summary>
        event Func<ProcessMessageEventArgs<TEntity>, CancellationToken, Task> ProcessMessageAsync;

        /// <summary>
        /// Definition of the error handling process.
        /// </summary>
        event Func<ProcessErrorEventArgs, Task> ProcessErrorAsync;
    }
}
