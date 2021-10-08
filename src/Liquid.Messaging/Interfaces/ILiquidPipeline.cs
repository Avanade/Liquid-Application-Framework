using System;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.Interfaces
{
    /// <summary>
    /// Execution Pipeline for message handlers.
    /// This class should be decorated to include behaviors in message handlers execution.
    /// </summary>
    public interface ILiquidPipeline
    {
        /// <summary>
        /// Execution process of message handlers. 
        /// </summary>
        /// <typeparam name="T">Message body type.</typeparam>
        /// <param name="message">Data of the message to be processed.</param>
        /// <param name="process">Specific message processing function.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        Task Execute<T>(ProcessMessageEventArgs<T> message, Func<ProcessMessageEventArgs<T>, CancellationToken, Task> process, CancellationToken cancellationToken);
    }
}
