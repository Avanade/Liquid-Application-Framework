using AutoMapper;
using Liquid.Core.Interfaces;
using Liquid.Core.Utils;
using Liquid.Messaging.Azure.Configuration;
using Liquid.Messaging.Azure.Parameters;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Extensions;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

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
        private readonly ILiquidContext _context;
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
        /// <param name="context">The context factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingConfiguration">The service bus messaging configuration.</param>
        /// <param name="serviceBusConsumerParameter">The service bus consumer parameter.</param>
        /// <exception cref="MessagingMissingConfigurationException"></exception>
        protected ServiceBusConsumer(IServiceProvider serviceProvider,
                                     IMediator mediator,
                                     IMapper mapper,
                                     ILiquidContext context,
                                     ILoggerFactory loggerFactory,
                                     ILiquidConfiguration<ServiceBusSettings> messagingConfiguration,
                                     ServiceBusConsumerParameter serviceBusConsumerParameter)
        {
            _serviceBusConsumerParameter = serviceBusConsumerParameter;
            _serviceProvider = serviceProvider;
            _context = context;
            _messagingSettings = messagingConfiguration.Settings ??
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

            try
            {

                var eventMessage = message.ContentType == CommonExtensions.GZipContentType
                    ? message.Body.GzipDecompress().ParseJson<TMessage>()
                    : message.Body.ParseJson<TMessage>();

                using (_serviceProvider.CreateScope())
                {

                    var headers = message.UserProperties;
                    if (headers != null)
                    {
                        AddContextHeaders(headers, _context);
                    }

                    //context.SetAggregationId(message.CorrelationId);
                    //context.SetMessageId(message.MessageId);
                    var messageProcessed = await _messageHandler.Invoke(eventMessage, headers, cancellationToken);
                    if (messageProcessed || _autoComplete)
                    {
                        await _client.CompleteAsync(message.SystemProperties.LockToken);
                    }
                    else
                    {
                        await _client.AbandonAsync(message.SystemProperties.LockToken);
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.LogError(ex, ex.Message);
                throw new MessagingConsumerException(ex);
            }
        }

        /// <summary>
        /// Adds the context headers.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <param name="context">The context.</param>
        private static void AddContextHeaders(IDictionary<string, object> headers, ILiquidContext context)
        {
            if (headers.TryGetValue("Culture", out var culture) && culture?.ToString().IsNotNullOrEmpty() == true)
                context.Upsert("culture", culture.ToString());
            if (headers.TryGetValue("Channel", out var channel) && channel?.ToString().IsNotNullOrEmpty() == true)
                context.Upsert("Channel", channel.ToString());
            if (headers.TryGetValue("CorrelationId", out var contextId) && contextId?.ToString().IsNotNullOrEmpty() == true)
                context.Upsert("CorrelationId", contextId.ToString().ToGuid());
            if (headers.TryGetValue("BusinessCorrelationId", out var businessContextId) && businessContextId?.ToString().IsNotNullOrEmpty() == true)
                context.Upsert("BusinessCorrelationId", businessContextId.ToString().ToGuid());
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