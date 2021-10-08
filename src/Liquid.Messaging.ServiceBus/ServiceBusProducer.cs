using Liquid.Messaging.Attributes;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Interfaces;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading.Tasks;

namespace Liquid.Messaging.ServiceBus
{
    ///<inheritdoc/>
    public class ServiceBusProducer<TEntity> : ILiquidProducer<TEntity>
    {
        private readonly IMessageSender _messageSender;

        /// <summary>
        /// Initialize a new instance of <see cref="ServiceBusConsumer{TEntity}"/>.
        /// </summary>
        /// <param name="factory">Service Bus client factory.</param>
        public ServiceBusProducer(IServiceBusFactory factory)
        {
            if (!typeof(TEntity).GetCustomAttributes(typeof(SettingsNameAttribute), true).Any())
            {
                throw new NotImplementedException($"The {nameof(SettingsNameAttribute)} attribute decorator must be added to class.");
            }

            var settings = typeof(TEntity).GetCustomAttribute<SettingsNameAttribute>(true);

            _messageSender = factory?.GetSender(settings.SettingsName) ?? throw new ArgumentNullException(nameof(factory));
        }

        ///<inheritdoc/>
        public async Task SendMessagesAsync(IEnumerable<TEntity> messageBodies)
        {
            try
            {
                await _messageSender.SendAsync(messageBodies.Select(e => ToMessage(e)).ToList());
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
                await _messageSender.SendAsync(ToMessage(messageBody, customProperties));
            }
            catch (Exception ex)
            {
                throw new MessagingProducerException(ex);
            }
        }

        private Message ToMessage(TEntity messageBody, IDictionary<string, object> customProperties = null)
        {
            var message = new Message(JsonSerializer.SerializeToUtf8Bytes(messageBody));

            if (customProperties != null)
            {
                foreach (var property in customProperties)
                {
                    message.UserProperties.Add(property);
                }
            }

            return message;
        }
    }
}
