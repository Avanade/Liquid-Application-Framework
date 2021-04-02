using System;
using Liquid.Messaging.Attributes;

namespace Liquid.Messaging.RabbitMq.Attributes
{
    /// <summary>
    /// RabbitMq Producer Attribute
    /// </summary>
    /// <seealso cref="Liquid.Messaging.Attributes.LightMessagingAttribute" />
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class RabbitMqProducerAttribute : LightMessagingAttribute
    {
        /// <summary>
        /// Gets the topic exchange.
        /// </summary>
        /// <value>
        /// The topic.
        /// </value>
        public string Exchange { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqProducerAttribute" /> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="exchange">The RabbitMq Exchange.</param>
        /// <param name="compressMessage">if set to <c>true</c> [compress message].</param>
        public RabbitMqProducerAttribute(string connectionId, string exchange, bool compressMessage = false) : base(connectionId, compressMessage)
        {
            Exchange = exchange;
        }
    }
}