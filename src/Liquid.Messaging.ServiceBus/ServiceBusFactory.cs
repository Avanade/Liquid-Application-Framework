using Azure.Messaging.ServiceBus;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.ServiceBus.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Liquid.Messaging.ServiceBus
{
    ///<inheritdoc/>
    public class ServiceBusFactory : IServiceBusFactory
    {
        private readonly ServiceBusSettings _options;

        /// <summary>
        /// Initialize a new instace of <see cref="ServiceBusFactory"/>
        /// </summary>
        /// <param name="settings">Configuration Providers</param>
        public ServiceBusFactory(IOptions<ServiceBusSettings> settings)
        {
            _options = settings.Value ?? throw new ArgumentNullException(nameof(settings));
        }

        ///<inheritdoc/>
        public ServiceBusProcessor GetProcessor(string settingsName)
        {
            try
            {
                var config = _options.Settings.FirstOrDefault(x => x.EntityPath == settingsName);

                var options = new ServiceBusProcessorOptions();

                options.ReceiveMode = config.PeekLockMode ? ServiceBusReceiveMode.PeekLock : ServiceBusReceiveMode.ReceiveAndDelete;

                options.MaxConcurrentCalls = config.MaxConcurrentCalls;

                var serviceBusClient = new ServiceBusClient(config.ConnectionString);

                ServiceBusProcessor processor;
                if (config.Subscription is null)
                {
                    processor = serviceBusClient.CreateProcessor(config.EntityPath, options);
                }
                else
                {                    
                    processor = serviceBusClient.CreateProcessor(config.EntityPath, config.Subscription, options);
                }

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
            var config = _options.Settings.FirstOrDefault(x => x.EntityPath == settingsName);

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
            var config = _options.Settings.FirstOrDefault(x => x.EntityPath == settingsName);

            try
            {
                var options = new ServiceBusReceiverOptions();

                options.ReceiveMode = config.PeekLockMode ? ServiceBusReceiveMode.PeekLock : ServiceBusReceiveMode.ReceiveAndDelete;
                
                var serviceBusClient = new ServiceBusClient(config.ConnectionString);                
                
                var receiver = serviceBusClient.CreateReceiver(config.EntityPath, options);                

                return receiver;
            }
            catch (Exception ex)
            {
                throw new MessagingMissingConfigurationException(ex, settingsName);
            }
        }
    }
}
