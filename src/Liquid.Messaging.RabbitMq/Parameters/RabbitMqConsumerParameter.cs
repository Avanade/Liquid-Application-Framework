using Liquid.Messaging.Parameters;
using Liquid.Messaging.RabbitMq.Configuration;
using System;

namespace Liquid.Messaging.RabbitMq.Parameters
{
    /// <summary>
    /// Azure Service Bus Consumer Attribute Class.
    /// </summary>
    public class RabbitMqConsumerParameter : LightMessagingParameter
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
        /// Gets or sets the advanced settings.
        /// </summary>
        /// <value>
        /// The advanced settings.
        /// </value>
        public RabbitMqParameterSettings AdvancedSettings { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqConsumerParameter" /> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="exchange">The topic exchange.</param>
        /// <param name="queue">The subscription queue.</param>
        /// <param name="advancedSettings">The advanced settings.</param>
        /// <exception cref="System.ArgumentNullException">
        /// exchange
        /// or
        /// queue
        /// </exception>
        public RabbitMqConsumerParameter(string connectionId, string exchange, string queue, RabbitMqParameterSettings advancedSettings = null) : base(connectionId)
        {
            Exchange = exchange ?? throw new ArgumentNullException(nameof(exchange));
            Queue = queue ?? throw new ArgumentNullException(nameof(queue));
            AdvancedSettings = advancedSettings;
        }
    }
}