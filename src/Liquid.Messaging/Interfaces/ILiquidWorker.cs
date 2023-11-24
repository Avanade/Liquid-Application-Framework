using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.Interfaces
{
    /// <summary>
    /// Liquid Worker Service interface.
    /// The implementation should be message handling process.
    /// </summary>
    /// <typeparam name="TEntity">Type of message body.</typeparam>
    public interface ILiquidWorker<TEntity>
    {
        /// <summary>
        /// This method is called when message handler gets a message.
        /// The implementation should return a task that represents 
        /// the process to be executed by the message handler.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        Task ProcessMessageAsync(ConsumerMessageEventArgs<TEntity> args, CancellationToken cancellationToken);
    }
}
