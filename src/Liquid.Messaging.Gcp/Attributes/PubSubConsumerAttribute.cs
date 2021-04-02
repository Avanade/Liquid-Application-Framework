using Liquid.Messaging.Attributes;

namespace Liquid.Messaging.Gcp.Attributes
{
    /// <summary>
    /// Google Cloud Pub/Sub Consumer Attribute Class.
    /// </summary>
    /// <seealso cref="Liquid.Messaging.Attributes.LightMessagingAttribute"/>
    public class PubSubConsumerAttribute : LightMessagingAttribute
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
        /// Gets a value indicating whether [automatic complete].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic complete]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoComplete { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PubSubConsumerAttribute" /> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="topic">The topic.</param>
        /// <param name="subscription">The subscription.</param>
        /// <param name="autoComplete">if set to <c>true</c> [automatic complete].</param>
        public PubSubConsumerAttribute(string connectionId, string topic, string subscription, bool autoComplete) : base(connectionId)
        {
            Topic = topic;
            Subscription = subscription;
            AutoComplete = autoComplete;
        }
    }
}