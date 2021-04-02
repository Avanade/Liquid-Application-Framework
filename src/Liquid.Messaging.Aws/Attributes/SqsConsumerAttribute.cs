using Liquid.Messaging.Attributes;

namespace Liquid.Messaging.Aws.Attributes
{
    /// <summary>
    /// AWS SQS Consumer Attribute Class.
    /// </summary>
    /// <seealso cref="LightMessagingAttribute" />
    public class SqsConsumerAttribute : LightMessagingAttribute
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
        /// Initializes a new instance of the <see cref="SqsConsumerAttribute" /> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="queue">The queue.</param>
        /// <param name="autoComplete">if set to <c>true</c> [automatic complete].</param>
        public SqsConsumerAttribute(string connectionId, string queue, bool autoComplete = false) : base(connectionId)
        {
            Queue = queue;
            AutoComplete = autoComplete;
        }
    }
}