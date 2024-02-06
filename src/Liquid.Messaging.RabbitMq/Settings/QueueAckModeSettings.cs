using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Messaging.RabbitMq.Settings
{

    /// <summary>
    /// Queue Ack Mode Settings
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class QueueAckModeSettings
    {

        /// <summary>
        /// Gets or sets a value indicating whether the message should requeued.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [requeue]; otherwise, <c>false</c>.
        /// </value>
        public bool Requeue { get; set; }

        /// <summary>
        /// Defines the AckMode
        /// </summary>
        public QueueAckModeEnum QueueAckMode { get; set; }

    }
}
