using Liquid.Core.Interfaces;
using Liquid.Core.Utils;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Extensions;
using Liquid.Messaging.RabbitMq.Configuration;
using Liquid.Messaging.RabbitMq.Parameters;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Liquid.Messaging.RabbitMq
{
    /// <summary>
    /// RabbitMq Producer Class.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message object.</typeparam>
    /// <seealso cref="ILightProducer{TMessage}" />
    public class RabbitMqProducer<TMessage> : ILightProducer<TMessage>
    {
        private readonly ILogger _logger;
        private readonly ILiquidContext _context;
        private readonly RabbitMqSettings _messagingSettings;
        private readonly RabbitMqProducerParameter _rabbitMqProducerParameter;
        private IModel _channelModel;


        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqProducer{TMessage}"/> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingConfiguration">The messaging configuration.</param>
        /// <param name="rabbitMqProducerParameter">The rabbit mq producer parameter.</param>
        /// <exception cref="MessagingMissingConfigurationException"></exception>
        public RabbitMqProducer(ILiquidContext contextFactory,
                                ILoggerFactory loggerFactory,
                                ILiquidConfiguration<RabbitMqSettings> messagingConfiguration,
                                RabbitMqProducerParameter rabbitMqProducerParameter)
        {
            _rabbitMqProducerParameter = rabbitMqProducerParameter;
            _messagingSettings = messagingConfiguration?.Settings ??
                    throw new MessagingMissingConfigurationException(_rabbitMqProducerParameter.ConnectionId);
            _context = contextFactory;
            _logger = loggerFactory.CreateLogger(typeof(RabbitMqProducer<TMessage>).FullName);
            InitializeClient();
        }

        /// <summary>
        /// Initializes the RabbitMq Client.
        /// </summary>
        private void InitializeClient()
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(_messagingSettings.ConnectionString),
                RequestedHeartbeat = TimeSpan.FromSeconds(_messagingSettings?.RequestHeartBeatInSeconds ?? 60),
                AutomaticRecoveryEnabled = _messagingSettings?.AutoRecovery ?? true
            };

            var connection = connectionFactory.CreateConnection();
            _channelModel = connection.CreateModel();
            _channelModel.ExchangeDeclare(_rabbitMqProducerParameter.Exchange,
                _rabbitMqProducerParameter.AdvancedSettings?.ExchangeType ?? "direct",
                _rabbitMqProducerParameter.AdvancedSettings?.Durable ?? false,
                _rabbitMqProducerParameter.AdvancedSettings?.AutoDelete ?? false,
                _rabbitMqProducerParameter.AdvancedSettings?.ExchangeArguments);
        }

        /// <summary>
        /// Sends the message asynchronous.
        /// </summary>
        /// <param name="message">The message object.</param>
        /// <param name="customHeaders">The message custom headers.</param>
        /// <exception cref="ArgumentNullException">message</exception>
        /// <exception cref="MessagingProducerException"></exception>
        public async Task SendMessageAsync(TMessage message, IDictionary<string, object> customHeaders = null)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (customHeaders == null) customHeaders = new Dictionary<string, object>();

            try
            {
                var context = _context;

                await Task.Run(() =>
                {
                    var aggregationId = context.Get("AggregationId").ToString();
                    var messageId = Guid.NewGuid().ToString();

                    foreach (var item in context.current)
                    {
                        customHeaders.TryAdd(item.Key, item.Value);
                    }

                    var messageProperties = _channelModel.CreateBasicProperties();

                    messageProperties.Persistent = _rabbitMqProducerParameter.AdvancedSettings?.Persistent ?? false;
                    messageProperties.Expiration = _rabbitMqProducerParameter.AdvancedSettings?.Expiration ?? "30000";
                    messageProperties.Headers = customHeaders;
                    messageProperties.CorrelationId = aggregationId;
                    messageProperties.MessageId = messageId;

                    var messageBytes = !_rabbitMqProducerParameter.CompressMessage ? message.ToJsonBytes() : message.ToJson().GzipCompress();

                    if (_rabbitMqProducerParameter.CompressMessage)
                    {
                        messageProperties.ContentType = CommonExtensions.GZipContentType;
                    }

                    _channelModel.BasicPublish(_rabbitMqProducerParameter.Exchange, string.Empty, messageProperties, messageBytes);

                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new MessagingProducerException(ex);
            }
        }
    }
}