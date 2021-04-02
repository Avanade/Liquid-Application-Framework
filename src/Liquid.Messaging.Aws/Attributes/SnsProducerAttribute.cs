using System;
using Liquid.Messaging.Attributes;

namespace Liquid.Messaging.Aws.Attributes
{
    /// <summary>
    /// AWS SNS Producer Attribute
    /// </summary>
    /// <seealso cref="LightMessagingAttribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class SnsProducerAttribute : LightMessagingAttribute
    {
        /// <summary>
        /// Gets the topic.
        /// </summary>
        /// <value>
        /// The topic.
        /// </value>
        public string Topic { get; }

        /// <summary>
        /// Gets the message structure.
        /// </summary>
        /// <value>
        /// The message structure.
        /// </value>
        public string MessageStructure { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqsProducerAttribute" /> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="topic">The topic name.</param>
        /// <param name="messageStructure">The message structure.</param>
        /// <param name="compressMessage">if set to <c>true</c> [compress message].</param>
        public SnsProducerAttribute(string connectionId, string topic, string messageStructure = "json", bool compressMessage = false) : base(connectionId, compressMessage)
        {
            Topic = topic;
            MessageStructure = messageStructure;
        }
    }
}