using System;
using Liquid.Messaging.Attributes;

namespace Liquid.Messaging.Kafka.Attributes
{
    /// <summary>
    /// Kafka Producer Attribute
    /// </summary>
    /// <seealso cref="Liquid.Messaging.Attributes.LightMessagingAttribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class KafkaProducerAttribute : LightMessagingAttribute
    {
        /// <summary>
        /// Gets the topic.
        /// </summary>
        /// <value>
        /// The topic.
        /// </value>
        public string Topic { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaProducerAttribute" /> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="topic">The topic.</param>
        /// <param name="compressMessage">if set to <c>true</c> [compress message].</param>
        public KafkaProducerAttribute(string connectionId, string topic, bool compressMessage = false) : base(connectionId, compressMessage)
        {
            Topic = topic;
        }
    }
}