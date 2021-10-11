using Liquid.Messaging.Exceptions;
using Liquid.Messaging.ServiceBus.Settings;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using System;

namespace Liquid.Messaging.ServiceBus
{
    ///<inheritdoc/>
    public class ServiceBusFactory : IServiceBusFactory
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initialize a new instace of <see cref="ServiceBusFactory"/>
        /// </summary>
        /// <param name="configuration">Configuration Providers</param>
        public ServiceBusFactory(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        ///<inheritdoc/>
        public IMessageReceiver GetReceiver(string settingsName)
        {   
            try
            {
                var config = _configuration.GetSection(settingsName).Get<ServiceBusSettings>();

                return new MessageReceiver(config.ConnectionString, config.EntityPath);
            }
            catch (Exception ex)
            {
                throw new MessagingMissingConfigurationException(ex, settingsName);
            }
        }

        ///<inheritdoc/>
        public IMessageSender GetSender(string settingsName)
        {
            var config = _configuration.GetSection(settingsName).Get<ServiceBusSettings>();

            try
            {
                return new MessageSender(config.ConnectionString, config.EntityPath);
            }
            catch (Exception ex)
            {
                throw new MessagingMissingConfigurationException(ex, settingsName);
            }
        }
    }
}
