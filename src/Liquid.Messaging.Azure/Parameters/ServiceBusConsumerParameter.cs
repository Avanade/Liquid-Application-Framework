using Liquid.Messaging.Parameters;
using System;

namespace Liquid.Messaging.Azure.Parameters
{
    /// <summary>
    /// Azure Service Bus Consumer Attribute Class.
    /// </summary>
    /// <seealso cref="LightMessagingParameter"/>
    public class ServiceBusConsumerParameter : LightMessagingParameter
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
        /// Gets or sets the maximum concurrent calls.
        /// </summary>
        /// <value>
        /// The maximum concurrent calls.
        /// </value>
        public int? MaxConcurrentCalls { get; set; }

        /// <summary>
        /// Gets or sets a value indicating to auto complete the received message.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic complete]; otherwise, <c>false</c>.
        /// </value>
        public bool? AutoComplete { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBusConsumerParameter" /> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="topic">The topic.</param>
        /// <param name="subscription">The subscription.</param>
        /// <param name="autoComplete">if set to <c>true</c> [automatic complete].</param>
        /// <param name="maxConcurrentCalls">The maximum concurrent calls.</param>
        /// <exception cref="ArgumentNullException">
        /// topic
        /// or
        /// subscription
        /// </exception>
        public ServiceBusConsumerParameter(string connectionId, string topic, string subscription, bool autoComplete = false, int maxConcurrentCalls = 1) : base(connectionId)
        {
            Topic = topic ?? throw new ArgumentNullException(nameof(topic));
            Subscription = subscription ?? throw new ArgumentNullException(nameof(subscription));
            AutoComplete = autoComplete;
            MaxConcurrentCalls = maxConcurrentCalls;
        }
    }
}