using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Confluent.Kafka;
using Liquid.Core.Configuration;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Core.Utils;
using Liquid.Messaging.Configuration;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Extensions;
using Liquid.Messaging.Kafka.Attributes;
using Liquid.Messaging.Kafka.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Liquid.Messaging.Kafka
{
    /// <summary>
    /// Azure Service Bus Consumer Class.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message object.</typeparam>
    /// <seealso cref="Liquid.Messaging.ILightConsumer{TMessage}" />
    /// <seealso cref="System.IDisposable" />
    public abstract class KafkaConsumer<TMessage> : ILightConsumer<TMessage>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MessagingSettings _messagingSettings;
        private readonly ILightContextFactory _contextFactory;
        private readonly ILightTelemetryFactory _telemetryFactory;
        private readonly KafkaConsumerAttribute _attribute;
        private readonly CancellationTokenSource _cancellationToken;
        private readonly Func<TMessage, IDictionary<string, object>, CancellationToken, Task<bool>> _messageHandler;
        private IConsumer<Ignore, string> _client;

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
        /// Initializes a new instance of the <see cref="KafkaConsumer{TMessage}" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingSettings">The messaging settings.</param>
        /// <exception cref="NotImplementedException">The {nameof(PubSubConsumerAttribute)} attribute decorator must be added to configuration class.</exception>
        protected KafkaConsumer(IServiceProvider serviceProvider,
                                 IMediator mediator,
                                 IMapper mapper,
                                 ILightContextFactory contextFactory,
                                 ILightTelemetryFactory telemetryFactory,
                                 ILoggerFactory loggerFactory,
                                 ILightConfiguration<List<MessagingSettings>> messagingSettings)
        {
            if (!GetType().GetCustomAttributes(typeof(KafkaConsumerAttribute), true).Any())
            {
                throw new NotImplementedException($"The {nameof(KafkaConsumerAttribute)} attribute decorator must be added to configuration class.");
            }
            _attribute = GetType().GetCustomAttribute<KafkaConsumerAttribute>(true);
            _telemetryFactory = telemetryFactory;
            _serviceProvider = serviceProvider;
            _contextFactory = contextFactory;
            _messagingSettings = messagingSettings?.Settings?.GetMessagingSettings(_attribute.ConnectionId) ?? throw new MessagingMissingConfigurationException("messaging");
            _cancellationToken = new CancellationTokenSource();
            MediatorService = mediator;
            MapperService = mapper;
            LogService = loggerFactory.CreateLogger(typeof(KafkaConsumer<TMessage>).FullName);
            _messageHandler = ConsumeAsync;
            Task.Run(StartProcessMessages);
        }

        /// <summary>
        /// Starts the process messages.
        /// </summary>
        /// <exception cref="MessagingConsumerException"></exception>
        private async Task StartProcessMessages()
        {
            try
            {
                //TODO: Rever forma de conexão e testar conectividade com o Kafka.
                var config = new ConsumerConfig
                {
                    SocketKeepaliveEnable = _messagingSettings.GetSocketKeepAlive(),
                    SocketTimeoutMs = _messagingSettings.GetTimeout(),
                    BootstrapServers = _messagingSettings.ConnectionString,
                    ClientId = _messagingSettings.Id,
                    EnableAutoCommit = false //this is done for fine tuning below
                };

                _client = new ConsumerBuilder<Ignore, string>(config).Build();
                _client.Subscribe(_attribute.Topic);

                //Polling messages from Kafka
                while (!_cancellationToken.Token.IsCancellationRequested)
                {
                    var response = _client.Consume(_cancellationToken.Token);
                    await ProcessMessageAsync(response);
                }
            }
            catch (Exception ex)
            {
                LogService.LogError(ex, ex.Message);
                throw new MessagingConsumerException(ex);
            }
        }

        /// <summary>
        /// Processes the message asynchronous.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <exception cref="MessagingConsumerException"></exception>
        private async Task ProcessMessageAsync(ConsumeResult<Ignore, string> response)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                var telemetryKey = $"KafkaConsumer_{_attribute.Topic}";

                telemetry.AddContext($"ConsumeMessage_{nameof(TMessage)}");
                telemetry.StartTelemetryStopWatchMetric(telemetryKey);

                var headers = response.Message.Headers.GetCustomHeaders();


                var eventMessage = headers.ContainsKey(CommonExtensions.ContentTypeHeader) &&
                                   headers[CommonExtensions.ContentTypeHeader].ToString().Equals(CommonExtensions.GZipContentType, StringComparison.InvariantCultureIgnoreCase)
                    ? Encoding.UTF8.GetBytes(response.Message.Value).GzipDecompress().ParseJson<TMessage>()
                    : response.Message.Value.ParseJson<TMessage>();

                using (_serviceProvider.CreateScope())
                {
                    var context = _contextFactory.GetContext();

                    AddContextHeaders(headers, context);

                    var messageProcessed = await _messageHandler.Invoke(eventMessage, headers, _cancellationToken.Token);
                    if (messageProcessed || _attribute.AutoComplete)
                    {
                        try
                        {
                            _client.Commit(response);
                        }
                        catch (KafkaException e)
                        {
                            Console.WriteLine($"Commit error: {e.Error.Reason}");
                        }
                    }

                    telemetry.CollectTelemetryStopWatchMetric(telemetryKey, new
                    {
                        consumer = telemetryKey,
                        messageType = typeof(TMessage).FullName,
                        aggregationId = context.GetAggregationId(),
                        messageId = headers.ContainsKey("liquidMessageId") ? headers["liquidMessageId"].ToString() : string.Empty,
                        message = eventMessage,
                        processed = messageProcessed,
                        autoComplete = _attribute.AutoComplete,
                        headers
                    });
                }
            }
            catch (Exception ex)
            {
                LogService.LogError(ex, ex.Message);
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
            if (headers.TryGetValue("liquidCulture", out var culture) && culture?.ToString().IsNotNullOrEmpty() == true)
                context.SetCulture(culture.ToString());
            if (headers.TryGetValue("liquidChannel", out var channel) && channel?.ToString().IsNotNullOrEmpty() == true)
                context.SetChannel(channel.ToString());
            if (headers.TryGetValue("liquidCorrelationId", out var contextId) && contextId?.ToString().IsNotNullOrEmpty() == true)
                context.SetContextId(contextId.ToString().ToGuid());
            if (headers.TryGetValue("liquidBusinessCorrelationId", out var businessContextId) && businessContextId?.ToString().IsNotNullOrEmpty() == true)
                context.SetBusinessContextId(businessContextId.ToString().ToGuid());
            if (headers.TryGetValue("liquidAggregationId", out var aggregationId) && aggregationId?.ToString().IsNotNullOrEmpty() == true)
                context.SetAggregationId(aggregationId.ToString());
            if (headers.TryGetValue("liquidMessageId", out var messageId) && messageId?.ToString().IsNotNullOrEmpty() == true)
                context.SetAggregationId(messageId.ToString());
        }
    }
}