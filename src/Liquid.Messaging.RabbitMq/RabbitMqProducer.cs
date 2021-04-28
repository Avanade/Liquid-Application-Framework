using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Liquid.Core.Configuration;
using Liquid.Core.Utils;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Messaging.Configuration;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Extensions;
using Liquid.Messaging.RabbitMq.Attributes;
using Liquid.Messaging.RabbitMq.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;

namespace Liquid.Messaging.RabbitMq
{
    /// <summary>
    /// RabbitMq Producer Class.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message object.</typeparam>
    /// <seealso cref="Liquid.Messaging.ILightProducer{TMessage}" />
    public abstract class RabbitMqProducer<TMessage> : ILightProducer<TMessage>
    {

        private readonly ILogger _logger;
        private readonly ILightContextFactory _contextFactory;
        private readonly List<MessagingSettings> _messagingSettings;
        private readonly RabbitMqProducerAttribute _attribute;
        private readonly ILightTelemetryFactory _telemetryFactory;
        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channelModel;

        /// <summary>
        /// Gets the rabbit mq settings.
        /// </summary>
        /// <value>
        /// The rabbit mq settings.
        /// </value>
        public abstract RabbitMqSettings RabbitMqSettings { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqProducer{TMessage}" /> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingSettings">The messaging settings.</param>
        /// <exception cref="NotImplementedException">The {nameof(RabbitMqProducerAttribute)} attribute decorator must be added to configuration class.</exception>
        protected RabbitMqProducer(ILightContextFactory contextFactory,
                                    ILightTelemetryFactory telemetryFactory,
                                    ILoggerFactory loggerFactory,
                                    ILightConfiguration<List<MessagingSettings>> messagingSettings)
        {
            if (!GetType().GetCustomAttributes(typeof(RabbitMqProducerAttribute), true).Any())
            {
                throw new NotImplementedException($"The {nameof(RabbitMqProducerAttribute)} attribute decorator must be added to configuration class.");
            }
            _attribute = GetType().GetCustomAttribute<RabbitMqProducerAttribute>(true);
            _telemetryFactory = telemetryFactory;
            _messagingSettings = messagingSettings?.Settings ?? throw new MessagingMissingConfigurationException("messaging");
            _contextFactory = contextFactory;
            _logger = loggerFactory.CreateLogger(typeof(RabbitMqProducer<TMessage>).FullName);
            InitializeClient();
        }

        /// <summary>
        /// Initializes the RabbitMq Client.
        /// </summary>
        private void InitializeClient()
        {
            var messagingSettings = _messagingSettings.GetMessagingSettings(_attribute.ConnectionId);
            _connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(messagingSettings.ConnectionString),
                RequestedHeartbeat = TimeSpan.FromSeconds(RabbitMqSettings?.RequestHeartBeatInSeconds ?? 60),
                AutomaticRecoveryEnabled = RabbitMqSettings?.AutoRecovery ?? true
            };

            _connection = _connectionFactory.CreateConnection();
            _channelModel = _connection.CreateModel();
            _channelModel.ExchangeDeclare(_attribute.Exchange,
                RabbitMqSettings?.ExchangeType ?? "direct",
                RabbitMqSettings?.Durable ?? false,
                RabbitMqSettings?.AutoDelete ?? false,
                RabbitMqSettings?.ExchangeArguments);
        }

        /// <summary>
        /// Sends the message asynchronous.
        /// </summary>
        /// <param name="message">The message object.</param>
        /// <param name="customHeaders">The message custom headers.</param>
        /// <exception cref="System.ArgumentNullException">message</exception>
        /// <exception cref="MessagingProducerException"></exception>
        public async Task SendMessageAsync(TMessage message, IDictionary<string, object> customHeaders = null)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            var telemetry = _telemetryFactory.GetTelemetry();
            if (customHeaders == null) customHeaders = new Dictionary<string, object>();

            try
            {
                var context = _contextFactory.GetContext();
                var telemetryKey = $"RabbitMqProducer_{_attribute.Exchange}";
                
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

                    messageProperties.Persistent = RabbitMqSettings?.Persistent ?? false;
                    messageProperties.Expiration = RabbitMqSettings?.Expiration ?? "30000";
                    messageProperties.Headers = customHeaders;
                    messageProperties.CorrelationId = aggregationId;
                    messageProperties.MessageId = messageId;                    

                    var messageBytes = !_attribute.CompressMessage ? message.ToJsonBytes() : message.ToJson().GzipCompress();

                    if (_attribute.CompressMessage)
                    {
                        messageProperties.ContentType = CommonExtensions.GZipContentType;
                    }

                    _channelModel.BasicPublish(_attribute.Exchange, string.Empty, messageProperties, messageBytes);

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