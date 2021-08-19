using Microsoft.Azure.ServiceBus.Core;

namespace Liquid.Messaging.ServiceBus.Interfaces
{
    /// <summary>
    /// Service Bus client Provider.
    /// </summary>
    public interface IServiceBusFactory
    {
        /// <summary>
        /// Initialize and return a new instance of <see cref="MessageSender"/>.
        /// </summary>
        IMessageSender GetSender();

        /// <summary>
        /// Initialize and return a new instance of <see cref="MessageReceiver"/>
        /// </summary>
        IMessageReceiver GetReceiver();
    }
}
