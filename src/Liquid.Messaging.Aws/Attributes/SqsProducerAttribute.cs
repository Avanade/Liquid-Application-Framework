using System;
using Liquid.Messaging.Attributes;

namespace Liquid.Messaging.Aws.Attributes
{
    /// <summary>
    /// AWS SQS Producer Attribute
    /// </summary>
    /// <seealso cref="LightMessagingAttribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class SqsProducerAttribute : LightMessagingAttribute
    {
        /// <summary>
        /// Gets the Queue.
        /// </summary>
        /// <value>
        /// The topic.
        /// </value>
        public string Queue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqsProducerAttribute" /> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="queue">The queue name.</param>
        /// <param name="compressMessage">if set to <c>true</c> [compress message].</param>
        public SqsProducerAttribute(string connectionId, string queue, bool compressMessage = false) : base(connectionId, compressMessage)
        {
            Queue = queue;
        }
    }
}