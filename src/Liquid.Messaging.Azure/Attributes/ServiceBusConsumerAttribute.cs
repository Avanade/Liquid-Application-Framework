using Liquid.Messaging.Attributes;

namespace Liquid.Messaging.Azure.Attributes
{
    /// <summary>
    /// Azure Service Bus Consumer Attribute Class.
    /// </summary>
    /// <seealso cref="Liquid.Messaging.Attributes.LightMessagingAttribute"/>
    public class ServiceBusConsumerAttribute : LightMessagingAttribute
    {
        /// <summary>
        /// Gets the topic.
        /// </summary>
        /// <value>
        /// The topic.
        /// </value>
        public string Topic { get; }

        /// <summary>
        /// Gets the subscription.
        /// </summary>
        /// <value>
        /// The subscription.
        /// </value>
        public string Subscription { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBusConsumerAttribute" /> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="topic">The topic.</param>
        /// <param name="subscription">The subscription.</param>
        public ServiceBusConsumerAttribute(string connectionId, string topic, string subscription) : base(connectionId)
        {
            Topic = topic;
            Subscription = subscription;
        }
    }
}