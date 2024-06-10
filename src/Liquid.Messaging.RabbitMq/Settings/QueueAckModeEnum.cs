using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Messaging.RabbitMq.Settings
{
    /// <summary>
    /// Defines the AckMode
    /// </summary>
    public enum QueueAckModeEnum
    {
        /// <summary>
        /// Basic Ack
        /// </summary>
        BasicAck = 0,

        /// <summary>
        /// Reject Ack
        /// </summary>
        BasicReject = 1

    }
}
