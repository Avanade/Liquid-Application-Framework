using Liquid.Core.Interfaces;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.ServiceBus.Interfaces;
using Liquid.Messaging.ServiceBus.Settings;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System;

namespace Liquid.Messaging.ServiceBus
{
    ///<inheritdoc/>
    public class ServiceBusFactory : IServiceBusFactory
    {
        private readonly ServiceBusSettings _options;

        /// <summary>
        /// Initialize a new instace of <see cref="ServiceBusFactory"/>
        /// </summary>
        /// <param name="options"></param>
        public ServiceBusFactory(ILiquidConfiguration<ServiceBusSettings> options)
        {
            _options = options?.Settings ?? throw new ArgumentNullException(nameof(options));
        }

        ///<inheritdoc/>
        public IMessageReceiver GetReceiver()
        {
            try
            {
                var receiveMode = _options.PeekLockMode ? ReceiveMode.PeekLock : ReceiveMode.ReceiveAndDelete;

                return new MessageReceiver(_options.ConnectionString, _options.EntityPath, receiveMode, null, _options.MaxConcurrentCalls);
            }
            catch (Exception ex)
            {
                throw new MessagingMissingConfigurationException(ex);
            }
        }

        ///<inheritdoc/>
        public IMessageSender GetSender()
        {
            try
            {
                return new MessageSender(_options.ConnectionString, _options.EntityPath);
            }
            catch (Exception ex)
            {
                throw new MessagingMissingConfigurationException(ex);
            }
        }
    }
}
