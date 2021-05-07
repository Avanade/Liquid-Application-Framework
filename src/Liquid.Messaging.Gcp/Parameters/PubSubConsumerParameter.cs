using Liquid.Messaging.Parameters;
using System;

namespace Liquid.Messaging.Gcp.Parameters
{
    /// <summary>
    /// Google Cloud Pub/Sub Consumer Parameter Class.
    /// </summary>
    /// <seealso cref="LightMessagingParameter"/>
    public class PubSubConsumerParameter : LightMessagingParameter
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
        /// Initializes a new instance of the <see cref="PubSubConsumerParameter" /> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="topic">The topic.</param>
        /// <param name="subscription">The subscription.</param>
        /// <param name="autoComplete">if set to <c>true</c> [automatic complete].</param>
        public PubSubConsumerParameter(string connectionId, string topic, string subscription, bool autoComplete = false) : base(connectionId)
        {
            Topic = topic ?? throw new ArgumentNullException(nameof(topic));
            Subscription = subscription ?? throw new ArgumentNullException(nameof(subscription));
            AutoComplete = autoComplete;
        }
    }
}