using Liquid.Messaging.Attributes;

namespace Liquid.Messaging.RabbitMq.Attributes
{
    /// <summary>
    /// Azure Service Bus Consumer Attribute Class.
    /// </summary>
    /// <seealso cref="Liquid.Messaging.Attributes.LightMessagingAttribute" />
    public class RabbitMqConsumerAttribute : LightMessagingAttribute
    {
        /// <summary>
        /// Gets the topic exchange.
        /// </summary>
        /// <value>
        /// The topic.
        /// </value>
        public string Exchange { get; }

        /// <summary>
        /// Gets the subscription queue.
        /// </summary>
        /// <value>
        /// The subscription.
        /// </value>
        public string Queue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqConsumerAttribute" /> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="exchange">The topic exchange.</param>
        /// <param name="queue">The subscription queue.</param>
        public RabbitMqConsumerAttribute(string connectionId, string exchange, string queue) : base(connectionId)
        {
            Exchange = exchange;
            Queue = queue;
        }
    }
}