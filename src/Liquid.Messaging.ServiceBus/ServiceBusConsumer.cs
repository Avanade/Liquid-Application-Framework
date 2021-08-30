using Liquid.Messaging.Attributes;
using Liquid.Messaging.Exceptions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System;
using System.Linq;
using System.Reflection;
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

        private readonly ILiquidPipeline _pipeline;

        ///<inheritdoc/>
        public event Func<ProcessMessageEventArgs<TEntity>, CancellationToken, Task> ProcessMessageAsync;

        ///<inheritdoc/>
        public event Func<ProcessErrorEventArgs, Task> ProcessErrorAsync;

        /// <summary>
        /// Initilize an instance of <see cref="ServiceBusConsumer{TEntity}"/>
        /// </summary>
        /// <param name="factory">Service Bus client factory.</param>
        /// <param name="pipeline">Liquid message handlers pipeline.</param>
        public ServiceBusConsumer(IServiceBusFactory factory, ILiquidPipeline pipeline)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
        }

        ///<inheritdoc/>
        public void RegisterMessageHandler()
        {
            if (!typeof(TEntity).GetCustomAttributes(typeof(SettingsNameAttribute), true).Any())
            {
                throw new NotImplementedException($"The {nameof(SettingsNameAttribute)} attribute decorator must be added to class.");
            }

            var settings = typeof(TEntity).GetCustomAttribute<SettingsNameAttribute>(true);

            _messageReceiver = _factory.GetReceiver(settings.SettingsName);

            _messageReceiver.RegisterMessageHandler(MessageHandler, new MessageHandlerOptions(ErrorHandler));
        }

        /// <summary>
        /// Process incoming messages.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <param name="cancellationToken"> Propagates notification that operations should be canceled.</param>
        protected async Task MessageHandler(Message message, CancellationToken cancellationToken)
        {
            await _pipeline.Execute(GetEventArgs(message), ProcessMessageAsync, cancellationToken);

            if (_messageReceiver.ReceiveMode == ReceiveMode.PeekLock)
            {
                await _messageReceiver.CompleteAsync(message.SystemProperties.LockToken);
            }
        }

        private Task ErrorHandler(ExceptionReceivedEventArgs args)
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
