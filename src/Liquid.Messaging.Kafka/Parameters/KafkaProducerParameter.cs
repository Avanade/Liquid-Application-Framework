using Liquid.Messaging.Parameters;
using System;

namespace Liquid.Messaging.Kafka.Parameters
{
    /// <summary>
    /// Kafka Producer Parameter
    /// </summary>
    /// <seealso cref="LightMessagingParameter" />
    public class KafkaProducerParameter : LightMessagingParameter
    {
        /// <summary>
        /// Gets the topic.
        /// </summary>
        /// <value>
        /// The topic.
        /// </value>
        public string Topic { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaProducerParameter" /> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="topic">The topic.</param>
        /// <param name="compressMessage">if set to <c>true</c> [compress message].</param>
        public KafkaProducerParameter(string connectionId, string topic, bool compressMessage = false) : base(connectionId, compressMessage)
        {
            Topic = topic ?? throw new ArgumentNullException(nameof(topic));
        }
    }
}