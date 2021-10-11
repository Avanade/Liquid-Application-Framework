using Microsoft.Azure.ServiceBus.Core;

namespace Liquid.Messaging.ServiceBus
{
    /// <summary>
    /// Service Bus client Provider.
    /// </summary>
    public interface IServiceBusFactory
    {
        /// <summary>
        /// Initialize and return a new instance of <see cref="MessageSender"/>.
        /// </summary>
        IMessageSender GetSender(string sectionName);

        /// <summary>
        /// Initialize and return a new instance of <see cref="MessageReceiver"/>
        /// </summary>
        IMessageReceiver GetReceiver(string sectionName);
    }
}
