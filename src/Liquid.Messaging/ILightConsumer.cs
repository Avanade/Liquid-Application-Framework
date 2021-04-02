using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging
{
    /// <summary>
    /// Light Message Consumer Interface.
    /// </summary>
    /// <typeparam name="TMessage">The type of the object.</typeparam>
    public interface ILightConsumer<in TMessage>
    {
        /// <summary>
        /// Consumes the message from subscription asynchronous.
        /// </summary>
        /// <param name="message">The message to be consumed.</param>
        /// <param name="headers">The custom headers of message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>true if the message will be acknowledged.</returns>
        Task<bool> ConsumeAsync(TMessage message, IDictionary<string, object> headers, CancellationToken cancellationToken);
    }
}