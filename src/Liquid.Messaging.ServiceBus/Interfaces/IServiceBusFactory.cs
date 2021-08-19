using Liquid.Core.Interfaces;
using Liquid.Messaging.ServiceBus.Settings;
using Microsoft.Azure.ServiceBus.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Liquid.Messaging.ServiceBus.Interfaces
{
    /// <summary>
    /// Service Bus client Provider.
    /// </summary>
    public interface IServiceBusFactory
    {
        /// <summary>
        /// Initialize and return a new instance of <see cref="MessageSender"/>.
        /// </summary>
        IMessageSender GetSender();

        /// <summary>
        /// Initialize and return a new instance of <see cref="MessageReceiver"/>
        /// </summary>
        IMessageReceiver GetReceiver();
    }
}
