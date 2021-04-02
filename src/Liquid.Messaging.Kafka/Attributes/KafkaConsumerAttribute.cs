using Liquid.Messaging.Attributes;

namespace Liquid.Messaging.Kafka.Attributes
{
    /// <summary>
    /// Kafka Consumer Attribute Class.
    /// </summary>
    /// <seealso cref="LightMessagingAttribute"/>
    public class KafkaConsumerAttribute : LightMessagingAttribute
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
        /// Initializes a new instance of the <see cref="KafkaConsumerAttribute" /> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="topic">The topic.</param>
        /// <param name="autoComplete">if set to <c>true</c> [automatic complete].</param>
        public KafkaConsumerAttribute(string connectionId, string topic, bool autoComplete) : base(connectionId)
        {
            Topic = topic;
            AutoComplete = autoComplete;
        }
    }
}