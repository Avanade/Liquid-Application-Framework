using Liquid.Messaging.Parameters;
using System;

namespace Liquid.Messaging.Aws.Parameters
{
    /// <summary>
    /// AWS SQS Consumer Parameter Class.
    /// </summary>
    /// <seealso cref="LightMessagingParameter" />
    public class SqsConsumerParameter : LightMessagingParameter
    {
        /// <summary>
        /// Gets the queue.
        /// </summary>
        /// <value>
        /// The queue.
        /// </value>
        public string Queue { get; }

        /// <summary>
        /// Gets a value indicating whether [automatic complete].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic complete]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoComplete { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqsConsumerParameter" /> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="queue">The queue.</param>
        /// <param name="autoComplete">if set to <c>true</c> [automatic complete].</param>
        public SqsConsumerParameter(string connectionId, string queue, bool autoComplete = false) : base(connectionId)
        {
            Queue = queue ?? throw new ArgumentNullException(nameof(queue));
            AutoComplete = autoComplete;
        }
    }
}