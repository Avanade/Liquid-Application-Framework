using Azure.Messaging.ServiceBus;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.ServiceBus.Settings;
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
        public ServiceBusProcessor GetProcessor(string settingsName)
        {
            try
            {
                var config = _configuration.GetSection(settingsName).Get<ServiceBusSettings>();

                var serviceBusClient = new ServiceBusClient(config.ConnectionString);

                var processor = serviceBusClient.CreateProcessor(config.EntityPath);

                return processor;
            }
            catch (Exception ex)
            {
                throw new MessagingMissingConfigurationException(ex, settingsName);
            }
        }

        ///<inheritdoc/>
        public ServiceBusSender GetSender(string settingsName)
        {
            var config = _configuration.GetSection(settingsName).Get<ServiceBusSettings>();

            try
            {
                var serviceBusClient = new ServiceBusClient(config.ConnectionString);
                var sender = serviceBusClient.CreateSender(config.EntityPath);

                return sender;
            }
            catch (Exception ex)
            {
                throw new MessagingMissingConfigurationException(ex, settingsName);
            }
        }

        ///<inheritdoc/>
        public ServiceBusReceiver GetReceiver(string settingsName)
        {
            var config = _configuration.GetSection(settingsName).Get<ServiceBusSettings>();

            try
            {
                var serviceBusClient = new ServiceBusClient(config.ConnectionString);
                var receiver = serviceBusClient.CreateReceiver(config.EntityPath);

                return receiver;
            }
            catch (Exception ex)
            {
                throw new MessagingMissingConfigurationException(ex, settingsName);
            }
        }
    }
}
