using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Liquid.Core.Utils;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Messaging.Configuration;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Extensions;
using Liquid.Messaging.RabbitMq.Parameters;
using Liquid.Messaging.RabbitMq.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

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
        private readonly ILightContextFactory _contextFactory;
        private readonly RabbitMqSettings _messagingSettings;
        private readonly RabbitMqProducerParameter _rabbitMqProducerParameter;
        private readonly ILightTelemetryFactory _telemetryFactory;
        private IModel _channelModel;


        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqProducer{TMessage}"/> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingConfiguration">The messaging configuration.</param>
        /// <param name="rabbitMqProducerParameter">The rabbit mq producer parameter.</param>
        /// <exception cref="MessagingMissingConfigurationException"></exception>
        public RabbitMqProducer(ILightContextFactory contextFactory,
                                ILightTelemetryFactory telemetryFactory,
                                ILoggerFactory loggerFactory,
                                ILightMessagingConfiguration<RabbitMqSettings> messagingConfiguration,
                                RabbitMqProducerParameter rabbitMqProducerParameter)
        {
            _rabbitMqProducerParameter = rabbitMqProducerParameter;
            _telemetryFactory = telemetryFactory;
            _messagingSettings = messagingConfiguration?.GetSettings(_rabbitMqProducerParameter.ConnectionId) ??
                    throw new MessagingMissingConfigurationException(_rabbitMqProducerParameter.ConnectionId);
            _contextFactory = contextFactory;
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
            var telemetry = _telemetryFactory.GetTelemetry();
            if (customHeaders == null) customHeaders = new Dictionary<string, object>();

            try
            {
                var context = _contextFactory.GetContext();
                var telemetryKey = $"RabbitMqProducer_{_rabbitMqProducerParameter.Exchange}";
                
                telemetry.AddContext($"SendMessage_{nameof(TMessage)}");
                
                await Task.Run(() =>
                {
                    telemetry.StartTelemetryStopWatchMetric(telemetryKey);
                    var aggregationId = context.GetAggregationId();
                    var messageId = Guid.NewGuid().ToString();

                    customHeaders.TryAdd("liquidCulture", context.ContextCulture);
                    customHeaders.TryAdd("liquidChannel", context.ContextChannel);
                    customHeaders.TryAdd("liquidCorrelationId", context.ContextId.ToString());
                    customHeaders.TryAdd("liquidBusinessCorrelationId", context.BusinessContextId.ToString());

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

                    telemetry.CollectTelemetryStopWatchMetric(telemetryKey, new
                    {
                        producer = telemetryKey,
                        messageType = typeof(TMessage).FullName,
                        aggregationId,
                        messageId,
                        message,
                        headers = customHeaders
                    });
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new MessagingProducerException(ex);
            }
            finally
            {
                telemetry.RemoveContext($"SendMessage_{nameof(TMessage)}");
            }
        }
    }
}