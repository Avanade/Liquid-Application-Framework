using System;
using Liquid.Messaging.Parameters;

namespace Liquid.Messaging.Aws.Parameters
{
    /// <summary>
    /// AWS SQS Producer Attribute
    /// </summary>
    /// <seealso cref="LightMessagingParameter" />
    public class SqsProducerParameter : LightMessagingParameter
    {
        /// <summary>
        /// Gets the Queue.
        /// </summary>
        /// <value>
        /// The topic.
        /// </value>
        public string Queue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqsProducerParameter" /> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="queue">The queue name.</param>
        /// <param name="compressMessage">if set to <c>true</c> [compress message].</param>
        public SqsProducerParameter(string connectionId, string queue, bool compressMessage = false) : base(connectionId, compressMessage)
        {
            Queue = queue ?? throw new ArgumentNullException(nameof(queue));
        }
    }
}