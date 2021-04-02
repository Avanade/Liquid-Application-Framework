using System.Collections.Generic;
using System.Threading.Tasks;

namespace Liquid.Messaging
{
    /// <summary>
    /// Light Message Producer Interface.
    /// </summary>
    /// <typeparam name="TMessage">The type of the object.</typeparam>
    public interface ILightProducer<in TMessage>
    {
        /// <summary>
        /// Sends the message asynchronous.
        /// </summary>
        /// <param name="message">The message object.</param>
        /// <param name="customHeaders">The message custom headers.</param>
        /// <returns></returns>
        Task SendMessageAsync(TMessage message, IDictionary<string, object> customHeaders = null);
    }
}