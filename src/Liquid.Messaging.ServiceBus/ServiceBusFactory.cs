using Liquid.Core.Interfaces;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.ServiceBus.Settings;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System;

namespace Liquid.Messaging.ServiceBus
{
    ///<inheritdoc/>
    public class ServiceBusFactory : IServiceBusFactory
    {
        private readonly ILiquidConfiguration<ServiceBusSettings> _options;

        /// <summary>
        /// Initialize a new instace of <see cref="ServiceBusFactory"/>
        /// </summary>
        /// <param name="options"></param>
        public ServiceBusFactory(ILiquidConfiguration<ServiceBusSettings> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        ///<inheritdoc/>
        public IMessageReceiver GetReceiver(string settingsName)
        {
            var config = _options.Settings.GetSettings(settingsName);

            try
            {
                var receiveMode = config.PeekLockMode ? ReceiveMode.PeekLock : ReceiveMode.ReceiveAndDelete;

                return new MessageReceiver(config.ConnectionString, config.EntityPath, receiveMode, null, config.MaxConcurrentCalls);
            }
            catch (Exception ex)
            {
                throw new MessagingMissingConfigurationException(ex);
            }
        }

        ///<inheritdoc/>
        public IMessageSender GetSender(string settingsName)
        {
            var config = _options.Settings.GetSettings(settingsName);

            try
            {
                return new MessageSender(config.ConnectionString, config.EntityPath);
            }
            catch (Exception ex)
            {
                throw new MessagingMissingConfigurationException(ex);
            }
        }
    }
}
