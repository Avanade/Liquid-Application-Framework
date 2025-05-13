using Azure.Messaging.ServiceBus;
using Liquid.Core.Entities;
using Liquid.Core.Exceptions;
using Liquid.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.ServiceBus
{
    ///<inheritdoc/>
    public class ServiceBusConsumer<TEntity> : ILiquidConsumer<TEntity>
    {
        private ServiceBusProcessor _messageProcessor;

        private readonly IServiceBusFactory _factory;

        private readonly string _settingsName;

        ///<inheritdoc/>
        public event Func<ConsumerMessageEventArgs<TEntity>, CancellationToken, Task> ConsumeMessageAsync;

        ///<inheritdoc/>
        public event Func<ConsumerErrorEventArgs, Task> ProcessErrorAsync;

        /// <summary>
        /// Initilize an instance of <see cref="ServiceBusConsumer{TEntity}"/>
        /// </summary>
        /// <param name="factory">Service Bus client factory.</param>
        /// <param name="settingsName">Configuration section name for this service instance.</param>
        public ServiceBusConsumer(IServiceBusFactory factory, string settingsName)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _settingsName = settingsName ?? throw new ArgumentNullException(nameof(settingsName));
        }

        ///<inheritdoc/>
        public async Task RegisterMessageHandler(CancellationToken cancellationToken = default)
        {
            if (ConsumeMessageAsync is null)
            {
                throw new NotImplementedException($"The {nameof(ProcessErrorAsync)} action must be added to class.");
            }

            _messageProcessor = _factory.GetProcessor(_settingsName);

            ProcessErrorAsync += ProcessError;

            _messageProcessor.ProcessMessageAsync += MessageHandler;
            _messageProcessor.ProcessErrorAsync += ErrorHandler;

            await _messageProcessor.StartProcessingAsync(cancellationToken);
        }

        /// <summary>
        /// Process incoming messages.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        protected async Task MessageHandler(ProcessMessageEventArgs message)
        {
            await ConsumeMessageAsync(GetEventArgs(message.Message), new CancellationToken());
        }

        /// <summary>
        /// Process exception from message handler.
        /// </summary>
        /// <param name="args"></param>
        protected async Task ErrorHandler(ProcessErrorEventArgs args)
        {
            await ProcessErrorAsync(new ConsumerErrorEventArgs()
            {
                Exception = args.Exception 
            });
        }

        private ConsumerMessageEventArgs<TEntity> GetEventArgs(ServiceBusReceivedMessage message)
        {
            var data = JsonSerializer.Deserialize<TEntity>(Encoding.UTF8.GetString(message.Body));

            var headers = (IDictionary<string,object>)message.ApplicationProperties;

            return new ConsumerMessageEventArgs<TEntity> { Data = data, Headers = headers };
        }
        /// <summary>
        /// Process error from message handler.
        /// </summary>
        /// <param name="args"><see cref="ConsumerErrorEventArgs"/></param>
        /// <exception cref="MessagingConsumerException"></exception>
        protected static Task ProcessError(ConsumerErrorEventArgs args)
        {
            throw new MessagingConsumerException(args.Exception);
        }

    }
}
