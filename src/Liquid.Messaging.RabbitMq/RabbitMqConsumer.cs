using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Liquid.Core.Utils;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Messaging.Configuration;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Extensions;
using Liquid.Messaging.RabbitMq.Parameters;
using Liquid.Messaging.RabbitMq.Configuration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Liquid.Messaging.RabbitMq
{
    /// <summary>
    /// RabbitMq Consumer Adapter Class.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message object.</typeparam>
    /// <seealso cref="ILightConsumer{TMessage}" />
    /// <seealso cref="IDisposable" />
    public abstract class RabbitMqConsumer<TMessage> : ILightConsumer<TMessage>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILightContextFactory _contextFactory;
        private readonly RabbitMqSettings _messagingSettings;
        private readonly ILightTelemetryFactory _telemetryFactory;
        private readonly RabbitMqConsumerParameter _rabbitMqConsumerParameter;
        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channelModel;
        private bool _autoAck;

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        [ExcludeFromCodeCoverage]
        public ILogger LogService { get; }

        /// <summary>
        /// Gets the mapper service.
        /// </summary>
        /// <value>
        /// The mapper service.
        /// </value>
        [ExcludeFromCodeCoverage]
        public IMapper MapperService { get; }

        /// <summary>
        /// Gets the consumer mediator.
        /// </summary>
        /// <value>
        /// The mediator.
        /// </value>
        [ExcludeFromCodeCoverage]
        public IMediator MediatorService { get; }

        /// <summary>
        /// Consumes the message from  subscription asynchronous.
        /// </summary>
        /// <param name="message">The message to be consumed.</param>
        /// <param name="headers">The custom headers of message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public abstract Task<bool> ConsumeAsync(TMessage message, IDictionary<string, object> headers, CancellationToken cancellationToken);

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqConsumer{TMessage}" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingConfiguration">The messaging configuration.</param>
        /// <param name="rabbitMqConsumerParameter">The rabbit mq consumer parameter.</param>
        /// <exception cref="MessagingMissingConfigurationException"></exception>
        protected RabbitMqConsumer(IServiceProvider serviceProvider,
                                   IMediator mediator,
                                   IMapper mapper,
                                   ILightContextFactory contextFactory,
                                   ILightTelemetryFactory telemetryFactory,
                                   ILoggerFactory loggerFactory,
                                   ILightMessagingConfiguration<RabbitMqSettings> messagingConfiguration,
                                   RabbitMqConsumerParameter rabbitMqConsumerParameter)
        {
            _rabbitMqConsumerParameter = rabbitMqConsumerParameter;
            _serviceProvider = serviceProvider;
            _contextFactory = contextFactory;
            _messagingSettings = messagingConfiguration?.GetSettings(_rabbitMqConsumerParameter.ConnectionId) ??
                    throw new MessagingMissingConfigurationException(_rabbitMqConsumerParameter.ConnectionId);
            _telemetryFactory = telemetryFactory;
            MediatorService = mediator;
            MapperService = mapper;
            LogService = loggerFactory.CreateLogger(typeof(RabbitMqConsumer<TMessage>).FullName);

            InitializeClient();
        }

        /// <summary>
        /// Initializes the RabbitMq client.
        /// </summary>
        private void InitializeClient()
        {
            _autoAck = _rabbitMqConsumerParameter.AdvancedSettings?.AutoAck ?? false;
            _connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(_messagingSettings.ConnectionString),
                RequestedHeartbeat = TimeSpan.FromSeconds(_messagingSettings?.RequestHeartBeatInSeconds ?? 60),
                AutomaticRecoveryEnabled = _messagingSettings?.AutoRecovery ?? true
            };

            _connection = _connectionFactory.CreateConnection();
            _channelModel = _connection.CreateModel();
            if (_messagingSettings.Prefetch.HasValue &&
                _messagingSettings.PrefetchCount.HasValue &&
                _messagingSettings.Global.HasValue)
            {
                _channelModel.BasicQos(_messagingSettings.Prefetch.Value, _messagingSettings.PrefetchCount.Value, _messagingSettings.Global.Value);
            }

            RegisterMessageHandler(ConsumeAsync);
        }

        /// <summary>
        /// Registers the message handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <returns></returns>
        public void RegisterMessageHandler(Func<TMessage, IDictionary<string, object>, CancellationToken, Task<bool>> handler)
        {
            _channelModel.QueueDeclare(_rabbitMqConsumerParameter.Queue,
               _rabbitMqConsumerParameter.AdvancedSettings?.Durable ?? false,
               _rabbitMqConsumerParameter.AdvancedSettings?.Exclusive ?? true,
               _rabbitMqConsumerParameter.AdvancedSettings?.AutoDelete ?? false,
               _rabbitMqConsumerParameter.AdvancedSettings?.QueueArguments);
            _channelModel.QueueBind(_rabbitMqConsumerParameter.Queue, _rabbitMqConsumerParameter.Exchange, string.Empty);

            var consumer = new EventingBasicConsumer(_channelModel);

            consumer.Received += async (model, deliverEvent) => { await ProcessMessageAsync(handler, deliverEvent); };

            _channelModel.BasicConsume(_rabbitMqConsumerParameter.Queue, _autoAck, consumer);
        }

        /// <summary>
        /// Handles the event message.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="deliverEvent">The <see cref="BasicDeliverEventArgs" /> instance containing the event data.</param>
        /// <returns></returns>
        /// <exception cref="MessagingConsumerException"></exception>
        private async Task ProcessMessageAsync(Func<TMessage, IDictionary<string, object>, CancellationToken, Task<bool>> handler, BasicDeliverEventArgs deliverEvent)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                var telemetryKey = $"RabbitMqConsumer_{_rabbitMqConsumerParameter.Exchange}_{_rabbitMqConsumerParameter.Queue}";

                telemetry.AddContext($"ConsumeMessage_{nameof(TMessage)}");
                telemetry.StartTelemetryStopWatchMetric(telemetryKey);

                var eventMessage = deliverEvent.BasicProperties?.ContentType == CommonExtensions.GZipContentType
                    ? deliverEvent.Body.ToArray().GzipDecompress().ParseJson<TMessage>()
                    : deliverEvent.Body.ToArray().ParseJson<TMessage>();

                using (_serviceProvider.CreateScope())
                {
                    var context = _contextFactory.GetContext();

                    var headers = deliverEvent.BasicProperties?.Headers;
                    if (headers != null) AddContextHeaders(headers, context);

                    context.SetAggregationId(deliverEvent.BasicProperties?.CorrelationId);
                    context.SetMessageId(deliverEvent.BasicProperties?.MessageId);

                    var messageProcessed = await handler.Invoke(eventMessage, headers, CancellationToken.None);
                    if (messageProcessed || _autoAck)
                    {
                        _channelModel.BasicAck(deliverEvent.DeliveryTag, false);
                    }
                    else
                    {
                        _channelModel.BasicNack(deliverEvent.DeliveryTag, false, true);
                    }

                    telemetry.CollectTelemetryStopWatchMetric(telemetryKey, new
                    {
                        consumer = typeof(RabbitMqConsumer<TMessage>).FullName,
                        messageType = typeof(TMessage).FullName,
                        aggregationId = deliverEvent.BasicProperties?.CorrelationId,
                        messageId = deliverEvent.BasicProperties?.MessageId,
                        message = eventMessage,
                        processed = _autoAck || messageProcessed,
                        autoComplete = _autoAck,
                        headers
                    });
                }
            }
            catch (Exception ex)
            {
                throw new MessagingConsumerException(ex);
            }
            finally
            {
                telemetry.RemoveContext($"ConsumeMessage_{nameof(TMessage)}");
            }
        }

        /// <summary>
        /// Adds the context headers.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <param name="context">The context.</param>
        private static void AddContextHeaders(IDictionary<string, object> headers, ILightContext context)
        {

            if (headers.TryGetValue("liquidCulture", out var culture) && culture != null && ((byte[])culture).Any())
                context.SetCulture(Encoding.UTF8.GetString((byte[])culture));
            if (headers.TryGetValue("liquidChannel", out var channel) && channel != null && ((byte[])channel).Any())
                context.SetChannel(Encoding.UTF8.GetString((byte[])channel));
            if (headers.TryGetValue("liquidCorrelationId", out var contextId) && contextId != null && ((byte[])contextId).Any())
                context.SetContextId(Encoding.UTF8.GetString((byte[])contextId).ToGuid());
            if (headers.TryGetValue("liquidBusinessCorrelationId", out var businessContextId) && businessContextId != null && ((byte[])businessContextId).Any())
                context.SetBusinessContextId(Encoding.UTF8.GetString((byte[])businessContextId).ToGuid());
        }
    }
}