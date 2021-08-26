using Liquid.Messaging.Attributes;
using Liquid.Messaging.Exceptions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
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
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
        }

        /// <summary>
        /// Initialize handler for consume <typeparamref name="TEntity"/> messages from topic or queue.
        /// </summary>
        public void Start()
        {
            if (!typeof(TEntity).GetCustomAttributes(typeof(SettingsNameAttribute), true).Any())
            {
                throw new NotImplementedException($"The {nameof(SettingsNameAttribute)} attribute decorator must be added to class.");
            }

            var settings = typeof(TEntity).GetCustomAttribute<SettingsNameAttribute>(true);

            _messageReceiver = _factory.GetReceiver(settings.SettingsName);

            _messageReceiver.RegisterMessageHandler(MessageHandler, new MessageHandlerOptions(ErrorHandler));
        }

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
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();

            memStream.Write(message.Body, 0, message.Body.Length);
            memStream.Seek(0, SeekOrigin.Begin);

            var data = (TEntity)binForm.Deserialize(memStream);
            var headers = message.UserProperties;

            return new ProcessMessageEventArgs<TEntity> { Data = data, Headers = headers };
        }
    }
}
