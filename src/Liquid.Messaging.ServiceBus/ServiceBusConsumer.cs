using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Interfaces;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.ServiceBus
{
    ///<inheritdoc/>
    public class ServiceBusConsumer<TEntity> : ILiquidConsumer<TEntity>
    {
        private IMessageReceiver _messageReceiver;

        private readonly IServiceBusFactory _factory;

        private readonly string _settingsName;

        ///<inheritdoc/>
        public event Func<ProcessMessageEventArgs<TEntity>, CancellationToken, Task> ProcessMessageAsync;

        ///<inheritdoc/>
        public event Func<ProcessErrorEventArgs, Task> ProcessErrorAsync;

        /// <summary>
        /// Initilize an instance of <see cref="ServiceBusConsumer{TEntity}"/>
        /// </summary>
        /// <param name="factory">Service Bus client factory.</param>
        /// <param name="settingsName">Configuration section name for this service instance.</param>
        public ServiceBusConsumer(IServiceBusFactory factory, string settingsName)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _settingsName = settingsName;
        }

        ///<inheritdoc/>
        public void RegisterMessageHandler()
        {
            if (ProcessMessageAsync is null)
            {
                throw new NotImplementedException($"The {nameof(ProcessErrorAsync)} action must be added to class.");
            }                       

            _messageReceiver = _factory.GetReceiver(_settingsName);

            _messageReceiver.RegisterMessageHandler(MessageHandler, new MessageHandlerOptions(ErrorHandler));
        }

        /// <summary>
        /// Process incoming messages.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <param name="cancellationToken"> Propagates notification that operations should be canceled.</param>
        protected async Task MessageHandler(Message message, CancellationToken cancellationToken)
        {
            await ProcessMessageAsync(GetEventArgs(message), cancellationToken);
        }

        /// <summary>
        /// Process exception from message handler.
        /// </summary>
        /// <param name="args"></param>
        public Task ErrorHandler(ExceptionReceivedEventArgs args)
        {
            return ProcessErrorAsync(new ProcessErrorEventArgs()
            {
                Exception = new MessagingConsumerException(args.Exception)
            });
        }

        private ProcessMessageEventArgs<TEntity> GetEventArgs(Message message)
        {
            var data = JsonSerializer.Deserialize<TEntity>(Encoding.UTF8.GetString(message.Body));

            var headers = message.UserProperties;

            return new ProcessMessageEventArgs<TEntity> { Data = data, Headers = headers };
        }
    }
}
