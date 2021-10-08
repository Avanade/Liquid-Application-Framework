using System.Collections.Generic;
using System.Threading.Tasks;

namespace Liquid.Messaging.Interfaces
{
    /// <summary>
    /// Handle send messages process.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ILiquidProducer<TEntity>
    {
        /// <summary>
        /// Structures and processes sending a list of <typeparamref name="TEntity"/> of messages.
        /// </summary>
        /// <param name="messageBodies">Body of messages to be sent.</param>
        Task SendMessagesAsync(IEnumerable<TEntity> messageBodies);

        /// <summary>
        /// Structures and processes sending a <typeparamref name="TEntity"/> message with
        /// its headers <paramref name="customProperties"/>.
        /// </summary>
        /// <param name="messageBody">Body of message to be sent.</param>
        /// <param name="customProperties">Message header properties.</param>
        Task SendMessageAsync(TEntity messageBody, IDictionary<string, object> customProperties);
    }
}
