using System.Diagnostics.CodeAnalysis;

namespace Liquid.Messaging.RabbitMq.Settings
{
    /// <summary>
    /// RabbitMq Consumer configurations set.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RabbitMqConsumerSettings
    {
        /// <summary>
        /// Gets a value indicating whether [compress message].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [compress message]; otherwise, <c>false</c>.
        /// </value>
        public bool CompressMessage { get; set; }

        /// <summary>
        /// Gets the topic exchange.
        /// </summary>
        /// <value>
        /// The topic.
        /// </value>
        public string Exchange { get; set; }

        /// <summary>
        /// Gets the subscription queue.
        /// </summary>
        /// <value>
        /// The subscription.
        /// </value>
        public string Queue { get; set; }

        /// <summary>
        /// Gets or sets the advanced settings.
        /// </summary>
        /// <value>
        /// The advanced settings.
        /// </value>
        public AdvancedSettings AdvancedSettings { get; set; }

        /// <summary>
        /// Infra basic settings.
        /// </summary>
        public RabbitMqSettings QueueSettings { get; set; }
    }
}
