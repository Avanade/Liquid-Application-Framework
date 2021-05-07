using System;
using Liquid.Messaging.Parameters;
using Liquid.Messaging.RabbitMq.Configuration;

namespace Liquid.Messaging.RabbitMq.Parameters
{
    /// <summary>
    /// RabbitMq Producer Attribute
    /// </summary>
    /// <seealso cref="LightMessagingParameter" />
    public class RabbitMqProducerParameter : LightMessagingParameter
    {
        /// <summary>
        /// Gets the topic exchange.
        /// </summary>
        /// <value>
        /// The topic.
        /// </value>
        public string Exchange { get; }

        /// <summary>
        /// Gets or sets the advanced settings.
        /// </summary>
        /// <value>
        /// The advanced settings.
        /// </value>
        public RabbitMqParameterSettings AdvancedSettings { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqProducerParameter" /> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="exchange">The RabbitMq Exchange.</param>
        /// <param name="advancedSettings">The advanced settings.</param>
        /// <param name="compressMessage">if set to <c>true</c> [compress message].</param>
        /// <exception cref="System.ArgumentNullException">exchange</exception>
        public RabbitMqProducerParameter(string connectionId, string exchange, RabbitMqParameterSettings advancedSettings = null, bool compressMessage = false) : base(connectionId, compressMessage)
        {
            Exchange = exchange ?? throw new ArgumentNullException(nameof(exchange));
            AdvancedSettings = advancedSettings;
        }
    }
}