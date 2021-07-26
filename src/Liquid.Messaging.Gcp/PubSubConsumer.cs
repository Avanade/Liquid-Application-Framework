using AutoMapper;
using Google.Cloud.PubSub.V1;
using Grpc.Core;
using Liquid.Core.Interfaces;
using Liquid.Core.Utils;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Extensions;
using Liquid.Messaging.Gcp.Configuration;
using Liquid.Messaging.Gcp.Extensions;
using Liquid.Messaging.Gcp.Factories;
using Liquid.Messaging.Gcp.Parameters;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.Gcp
{
    /// <summary>
    /// Google Cloud Pub/Sub Consumer Class.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message object.</typeparam>
    /// <seealso cref="ILightConsumer{TMessage}" />
    /// <seealso cref="IDisposable" />
    public abstract class PubSubConsumer<TMessage> : ILightConsumer<TMessage>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly PubSubSettings _messagingSettings;
        private readonly ILiquidContext _context;
        private readonly PubSubConsumerParameter _pubSubConsumerParameter;
        private readonly IPubSubClientFactory _pubSubClientFactory;
        private SubscriberClient _client;

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
        /// Initializes a new instance of the <see cref="PubSubConsumer{TMessage}" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="context">The context factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="pubSubClientFactory">The pub sub client factory.</param>
        /// <param name="messagingConfiguration">The messaging configuration.</param>
        /// <param name="pubSubConsumerParameter">The pub sub consumer parameter.</param>
        /// <exception cref="MessagingMissingConfigurationException">messaging</exception>
        protected PubSubConsumer(IServiceProvider serviceProvider,
                                 IMediator mediator,
                                 IMapper mapper,
                                 ILiquidContext context,
                                 ILoggerFactory loggerFactory,
                                 IPubSubClientFactory pubSubClientFactory,
                                 ILiquidConfiguration<PubSubSettings> messagingConfiguration,
                                 PubSubConsumerParameter pubSubConsumerParameter)
        {
            _pubSubConsumerParameter = pubSubConsumerParameter;
            _serviceProvider = serviceProvider;
            _context = context;
            _pubSubClientFactory = pubSubClientFactory;
            _messagingSettings = messagingConfiguration?.Settings ??
                    throw new MessagingMissingConfigurationException(_pubSubConsumerParameter.ConnectionId);
            MediatorService = mediator;
            MapperService = mapper;
            LogService = loggerFactory.CreateLogger(typeof(PubSubConsumer<TMessage>).FullName);
            InitializeClient();
        }

        /// <summary>
        /// Initializes the client.
        /// </summary>
        private void InitializeClient()
        {
            var topicName = new TopicName(_messagingSettings.ProjectId, _pubSubConsumerParameter.Topic);
            var subscriptionName = new SubscriptionName(_messagingSettings.ProjectId, _pubSubConsumerParameter.Subscription);

            var subscriberService = _pubSubClientFactory.GetSubscriberServiceApiClient(_messagingSettings);

            try
            {
                subscriberService.CreateSubscription(subscriptionName, topicName, null, 60);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
            {
                LogService.LogWarning($"Subscription {_pubSubConsumerParameter.Topic}, from topic {_pubSubConsumerParameter.Topic} already exists.");
            }


            Task.Run(async () =>
            {
                _client = _pubSubClientFactory.GetSubscriberClient(_messagingSettings, subscriptionName);
                await ProcessMessagesAsync(ConsumeAsync);
            });

        }

        /// <summary>
        /// Processes the messages asynchronous.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public async Task ProcessMessagesAsync(Func<TMessage, IDictionary<string, object>, CancellationToken, Task<bool>> handler)
        {
            await _client.StartAsync(async (message, cancellationToken) =>
            {
                
                try
                {
                   

                    var eventMessage = message.Attributes.ContainsKey(CommonExtensions.ContentTypeHeader) &&
                                       message.Attributes[CommonExtensions.ContentTypeHeader].Equals(CommonExtensions.GZipContentType, StringComparison.InvariantCultureIgnoreCase)
                        ? message.Data.ToByteArray().GzipDecompress().ParseJson<TMessage>()
                        : message.Data.ToByteArray().ParseJson<TMessage>();

                    using (_serviceProvider.CreateScope())
                    {
                        var context = _context;

                        var headers = message.Attributes.GetCustomHeaders();

                        if (headers != null) { AddContextHeaders(headers, context); }

                        var messageProcessed = await handler.Invoke(eventMessage, headers, cancellationToken);

                        

                        return messageProcessed || _pubSubConsumerParameter.AutoComplete ?
                            await Task.FromResult(SubscriberClient.Reply.Ack) :
                            await Task.FromResult(SubscriberClient.Reply.Nack);
                    }
                }
                catch (Exception ex)
                {
                    LogService.LogError(ex, ex.Message);
                    throw new MessagingConsumerException(ex);
                }
            });
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
            if (headers.TryGetValue("AggregationId", out var aggregationId) && aggregationId?.ToString().IsNotNullOrEmpty() == true)
                context.Upsert("AggregationId", aggregationId.ToString());
        }


    }
}