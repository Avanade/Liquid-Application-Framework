using System.Diagnostics.CodeAnalysis;

namespace Liquid.Messaging.RabbitMq.Settings
{

    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RabbitMqSettings
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the request heart beat in seconds.
        /// </summary>
        /// <value>
        /// The request heart beat in seconds.
        /// </value>
        public ushort? RequestHeartBeatInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the prefetch count.
        /// </summary>
        /// <value>
        /// The prefetch count.
        /// </value>
        public ushort? PrefetchCount { get; set; }

        /// <summary>
        /// Gets or sets the prefetch.
        /// </summary>
        /// <value>
        /// The prefetch.
        /// </value>
        public uint? Prefetch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Qos is global.
        /// </summary>
        /// <value>
        ///   <c>true</c> if global; otherwise, <c>false</c>.
        /// </value>
        public bool? Global { get; set; }

        /// <summary>
        /// Gets or sets the auto recovery property of Rabbit to recover exchanges and queue bindings.
        /// </summary>
        /// <value>
        /// The auto recovery value.
        /// </value>
        public bool? AutoRecovery { get; set; }
    }
}
