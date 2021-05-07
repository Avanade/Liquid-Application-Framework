using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Core.Utils;
using Liquid.Messaging.Azure.Configuration;
using Liquid.Messaging.Azure.Parameters;
using Liquid.Messaging.Configuration;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Extensions;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Liquid.Messaging.Azure
{
    /// <summary>
    /// Azure Service Bus Consumer Class.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message object.</typeparam>
    /// <seealso cref="ILightConsumer{TMessage}" />
    /// <seealso cref="IDisposable" />
    public abstract class ServiceBusConsumer<TMessage> : ILightConsumer<TMessage>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ServiceBusSettings _messagingSettings;
        private readonly ILightContextFactory _contextFactory;
        private readonly ILightTelemetryFactory _telemetryFactory;
        private readonly ServiceBusConsumerParameter _serviceBusConsumerParameter;
        private SubscriptionClient _client;
        private bool _autoComplete;
        private Func<TMessage, IDictionary<string, object>, CancellationToken, Task<bool>> _messageHandler;

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
        /// Initializes a new instance of the <see cref="ServiceBusConsumer{TMessage}" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingConfiguration">The service bus messaging configuration.</param>
        /// <param name="serviceBusConsumerParameter">The service bus consumer parameter.</param>
        /// <exception cref="MessagingMissingConfigurationException"></exception>
        protected ServiceBusConsumer(IServiceProvider serviceProvider,
                                     IMediator mediator,
                                     IMapper mapper,
                                     ILightContextFactory contextFactory,
                                     ILightTelemetryFactory telemetryFactory,
                                     ILoggerFactory loggerFactory,
                                     ILightMessagingConfiguration<ServiceBusSettings> messagingConfiguration,
                                     ServiceBusConsumerParameter serviceBusConsumerParameter)
        {
            _serviceBusConsumerParameter = serviceBusConsumerParameter;
            _telemetryFactory = telemetryFactory;
            _serviceProvider = serviceProvider;
            _contextFactory = contextFactory;
            _messagingSettings = messagingConfiguration?.GetSettings(_serviceBusConsumerParameter.ConnectionId) ??
                    throw new MessagingMissingConfigurationException(_serviceBusConsumerParameter.ConnectionId);

            MediatorService = mediator;
            MapperService = mapper;
            LogService = loggerFactory.CreateLogger(typeof(ServiceBusConsumer<TMessage>).FullName);
            InitializeClient();
        }

        private void InitializeClient()
        {
            _client = new SubscriptionClient(_messagingSettings.ConnectionString, _serviceBusConsumerParameter.Topic, _serviceBusConsumerParameter.Subscription);
            _autoComplete = _serviceBusConsumerParameter?.AutoComplete ?? false;
            RegisterMessageHandler(ConsumeAsync);
        }

        /// <summary>
        /// Registers the message handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <returns></returns>
        public void RegisterMessageHandler(Func<TMessage, IDictionary<string, object>, CancellationToken, Task<bool>> handler)
        {
            _messageHandler = handler;
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = _serviceBusConsumerParameter?.MaxConcurrentCalls ?? 1,
                AutoComplete = _autoComplete
            };
            _client.RegisterMessageHandler(ProcessMessageAsync, messageHandlerOptions);
        }

        /// <summary>
        /// Processes the message asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task ProcessMessageAsync(Message message, CancellationToken cancellationToken)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                var telemetryKey = $"ServiceBusConsumer_{_serviceBusConsumerParameter.Topic}_{_serviceBusConsumerParameter.Subscription}";

                telemetry.AddContext($"ConsumeMessage_{nameof(TMessage)}");
                telemetry.StartTelemetryStopWatchMetric(telemetryKey);

                var eventMessage = message.ContentType == CommonExtensions.GZipContentType
                    ? message.Body.GzipDecompress().ParseJson<TMessage>()
                    : message.Body.ParseJson<TMessage>();

                using (_serviceProvider.CreateScope())
                {
                    var context = _contextFactory.GetContext();
                    var headers = message.UserProperties;
                    if (headers != null)
                    {
                        AddContextHeaders(headers, context);
                    }

                    context.SetAggregationId(message.CorrelationId);
                    context.SetMessageId(message.MessageId);
                    var messageProcessed = await _messageHandler.Invoke(eventMessage, headers, cancellationToken);
                    if (messageProcessed || _autoComplete)
                    {
                        await _client.CompleteAsync(message.SystemProperties.LockToken);
                    }
                    else
                    {
                        await _client.AbandonAsync(message.SystemProperties.LockToken);
                    }

                    telemetry.CollectTelemetryStopWatchMetric(telemetryKey, new
                    {
                        consumer = telemetryKey,
                        messageType = typeof(TMessage).FullName,
                        aggregationId = message.CorrelationId,
                        messageId = message.MessageId,
                        message = eventMessage,
                        processed = _autoComplete || messageProcessed,
                        autoComplete = _autoComplete,
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
        }

        /// <summary>
        /// Exceptions the received handler.
        /// </summary>
        /// <param name="exceptionReceivedEventArgs">The <see cref="ExceptionReceivedEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        /// <exception cref="MessagingConsumerException"></exception>
        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            throw new MessagingConsumerException(exceptionReceivedEventArgs.Exception);
        }
    }
}