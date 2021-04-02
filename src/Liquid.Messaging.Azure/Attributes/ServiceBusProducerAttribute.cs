using System;
using Liquid.Messaging.Attributes;

namespace Liquid.Messaging.Azure.Attributes
{
    /// <summary>
    /// Azure Service Bus Producer Attribute
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceBusProducerAttribute : LightMessagingAttribute
    {
        /// <summary>
        /// Gets the topic.
        /// </summary>
        /// <value>
        /// The topic.
        /// </value>
        public string Topic { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBusProducerAttribute" /> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="topic">The topic.</param>
        /// <param name="compressMessage">if set to <c>true</c> [compress message].</param>
        public ServiceBusProducerAttribute(string connectionId, string topic, bool compressMessage = false) : base(connectionId, compressMessage)
        {
            Topic = topic;
        }
    }
}