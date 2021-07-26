using Liquid.Core.Interfaces;
using Liquid.Messaging.ServiceBus.Interfaces;
using Liquid.Messaging.ServiceBus.Settings;
using Microsoft.Azure.ServiceBus.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Liquid.Messaging.ServiceBus
{
    public class ServiceBusFactory : IServiceBusFactory
    {
        private readonly ILiquidConfiguration<ServiceBusSettings> _options;

        public ServiceBusFactory(ILiquidConfiguration<ServiceBusSettings> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public IMessageReceiver GetReceiver()
        {           
            //TODO : Validação de nulos
            return new MessageReceiver(_options.Settings.ConnectionString, _options.Settings.EntityPath);
        }

        public IMessageSender GetSender()
        {
            //TODO : Validação de nulos
            return new MessageSender(_options.Settings.ConnectionString, _options.Settings.EntityPath);
        }
    }
}
