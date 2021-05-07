using Liquid.Messaging.Parameters;
using System;

namespace Liquid.Messaging.Kafka.Parameters
{
    /// <summary>
    /// Kafka Consumer Attribute Class.
    /// </summary>
    /// <seealso cref="LightMessagingParameter"/>
    public class KafkaConsumerParameter : LightMessagingParameter
    {
        /// <summary>
        /// Gets the topic.
        /// </summary>
        /// <value>
        /// The topic.
        /// </value>
        public string Topic { get; }

        /// <summary>
        /// Gets a value indicating whether [automatic complete].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic complete]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoComplete { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaConsumerParameter" /> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="topic">The topic.</param>
        /// <param name="autoComplete">if set to <c>true</c> [automatic complete].</param>
        public KafkaConsumerParameter(string connectionId, string topic, bool autoComplete = false) : base(connectionId)
        {
            Topic = topic ?? throw new ArgumentNullException(nameof(topic));
            AutoComplete = autoComplete;
        }
    }
}