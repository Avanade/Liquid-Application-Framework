using Liquid.Messaging.Exceptions;
using Liquid.Messaging.ServiceBus.Interfaces;
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

        private readonly ILiquidPipeline _pipeline;

        public event Func<ProcessMessageEventArgs<TEntity>, CancellationToken, Task> ProcessMessageAsync;
        public event Func<ProcessErrorEventArgs, Task> ProcessErrorAsync;

        /// <summary>
        /// Initilize an instance of <see cref="ServiceBusConsumer"/>
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="pipeline"></param>
        public ServiceBusConsumer(IServiceBusFactory factory, ILiquidPipeline pipeline)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _pipeline = pipeline;
        }

        /// <summary>
        /// Initialize handler for consume <typeparamref name="TEntity"/> messages from topic or queue.
        /// </summary>
        public void Start()
        {
            _messageReceiver = _factory.GetReceiver();
            _messageReceiver.RegisterMessageHandler(MessageHandler, new MessageHandlerOptions(ErrorHandler));
        }

        private async Task MessageHandler(Message message, CancellationToken cancellationToken)
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
