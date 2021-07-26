using Liquid.Core.Interfaces;
using Liquid.Messaging.ServiceBus.Settings;
using Microsoft.Azure.ServiceBus.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Liquid.Messaging.ServiceBus.Interfaces
{
    public interface IServiceBusFactory
    {
        IMessageSender GetSender();

        IMessageReceiver GetReceiver();
    }
}
