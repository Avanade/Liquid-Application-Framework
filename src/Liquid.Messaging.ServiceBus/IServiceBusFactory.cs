using Azure.Messaging.ServiceBus;

namespace Liquid.Messaging.ServiceBus
{
    /// <summary>
    /// Service Bus client Provider.
    /// </summary>
    public interface IServiceBusFactory
    {
        /// <summary>
        /// Initialize and return a new instance of <see cref="ServiceBusSender"/>.
        /// </summary>
        ServiceBusSender GetSender(string entityPath);

        /// <summary>
        /// Initialize and return a new instance of <see cref="ServiceBusProcessor"/>
        /// </summary>
        ServiceBusProcessor GetProcessor(string entityPath);

        /// <summary>
        /// Initialize and return a new instance of <see cref="ServiceBusReceiver"/>
        /// </summary>
        ServiceBusReceiver GetReceiver(string entityPath);
    }
}
