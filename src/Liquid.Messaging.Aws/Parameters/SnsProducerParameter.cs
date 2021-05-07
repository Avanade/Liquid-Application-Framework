using Liquid.Messaging.Parameters;
using System;

namespace Liquid.Messaging.Aws.Parameters
{
    /// <summary>
    /// AWS SNS Producer Parameter
    /// </summary>
    /// <seealso cref="LightMessagingParameter" />
    public class SnsProducerParameter : LightMessagingParameter
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
        /// Initializes a new instance of the <see cref="SnsProducerParameter" /> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="topic">The topic name.</param>
        /// <param name="messageStructure">The message structure.</param>
        /// <param name="compressMessage">if set to <c>true</c> [compress message].</param>
        public SnsProducerParameter(string connectionId, string topic, string messageStructure = "json", bool compressMessage = false) : base(connectionId, compressMessage)
        {
            Topic = topic ?? throw new ArgumentNullException(nameof(topic));
            MessageStructure = messageStructure;
        }
    }
}