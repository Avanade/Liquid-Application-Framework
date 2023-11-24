using Azure.Messaging.ServiceBus;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Liquid.Messaging.ServiceBus
{
    ///<inheritdoc/>
    public class ServiceBusProducer<TEntity> : ILiquidProducer<TEntity>
    {
        private readonly ServiceBusSender _messageSender;
        /// <summary>
        /// Initialize a new instance of <see cref="ServiceBusConsumer{TEntity}"/>.
        /// </summary>
        /// <param name="factory">Service Bus client factory.</param>
        /// <param name="settingsName">Configuration section name for this service instance.</param>
        public ServiceBusProducer(IServiceBusFactory factory, string settingsName)
        {
            if (settingsName is null) throw new ArgumentNullException(nameof(settingsName));
            _messageSender = factory?.GetSender(settingsName) ?? throw new ArgumentNullException(nameof(factory));
        }

        ///<inheritdoc/>
        public async Task SendMessagesAsync(IEnumerable<TEntity> messageBodies)
        {
            try
            {
                await _messageSender.SendMessagesAsync(messageBodies.Select(e => ToMessage(e)).ToList());
            }
            catch (Exception ex)
            {
                throw new MessagingProducerException(ex);
            }
        }

        ///<inheritdoc/>
        public async Task SendMessageAsync(TEntity messageBody, IDictionary<string, object> customProperties = null)
        {
            try
            {
                await _messageSender.SendMessageAsync(ToMessage(messageBody, customProperties));
            }
            catch (Exception ex)
            {
                throw new MessagingProducerException(ex);
            }
        }

        private ServiceBusMessage ToMessage(TEntity messageBody, IDictionary<string, object> customProperties = null)
        {
            var message = new ServiceBusMessage(JsonSerializer.SerializeToUtf8Bytes(messageBody));

            if (customProperties != null)
            {
                foreach (var property in customProperties)
                {
                    message.ApplicationProperties.Add(property);
                }
            }

            return message;
        }
    }
}
