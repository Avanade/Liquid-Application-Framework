using Liquid.Core.Extensions;
using Liquid.Core.Utils; 
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Extensions;
using Liquid.Messaging.Interfaces;
using Liquid.Messaging.RabbitMq.Settings;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Liquid.Messaging.RabbitMq
{
    ///<inheritdoc/>
    public class RabbitMqProducer<TEntity> : ILiquidProducer<TEntity>
    {
        private readonly RabbitMqProducerSettings _settings;
        private readonly IModel _channelModel;


        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqProducer{TMessage}"/> class.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="settings"></param>
        /// <exception cref="MessagingMissingConfigurationException"></exception>
        public RabbitMqProducer(IRabbitMqFactory factory, RabbitMqProducerSettings settings)
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            

            _channelModel = factory.GetSender(settings);
        }


        ///<inheritdoc/>
        public async Task SendMessageAsync(TEntity messageBody, IDictionary<string, object> customProperties = null)
        {
            if (customProperties == null) customProperties = new Dictionary<string, object>();
            try
            {
                await Task.Run(() =>
                {
                    var messageProperties = GetProperties(customProperties);

                    var messageBytes = !_settings.CompressMessage ? messageBody.ToJsonBytes() : messageBody.ToJsonString().GzipCompress();

                    _channelModel.BasicPublish(_settings.Exchange, string.Empty, messageProperties, messageBytes);

                });
            }
            catch (Exception ex)
            {
                throw new MessagingProducerException(ex);
            }
        }

        ///<inheritdoc/>
        public async Task SendMessagesAsync(IEnumerable<TEntity> messageBodies)
        {
            try
            {
                foreach (var message in messageBodies)
                {
                    await SendMessageAsync(message);
                }
            }
            catch (Exception ex)
            {
                throw new MessagingProducerException(ex);
            }
        }

        private IBasicProperties GetProperties(IDictionary<string, object> customProperties)
        {
            var messageProperties = _channelModel.CreateBasicProperties();

            var messageId = Guid.NewGuid().ToString();

            messageProperties.Persistent = _settings.AdvancedSettings?.Persistent ?? false;
            messageProperties.Expiration = _settings.AdvancedSettings?.Expiration ?? "30000";
            messageProperties.Headers = customProperties;
            messageProperties.MessageId = messageId;

            if (_settings.CompressMessage)
            {
                messageProperties.ContentType = CommonExtensions.GZipContentType;
            }

            return messageProperties;
        }
    }
}