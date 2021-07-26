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
    public class ServiceBusConsumer<TEvent> : ILiquidConsumer<TEvent>
    {
        private IMessageReceiver _messageReceiver;
        private readonly IServiceBusFactory _factory;

        public event Func<ProcessMessageEventArgs<TEvent>, CancellationToken, Task> ProcessMessageAsync;
        public event Func<ProcessErrorEventArgs, Task> ProcessErrorAsync;

        public ServiceBusConsumer(IServiceBusFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }
        public void Start()
        {
            _messageReceiver = _factory.GetReceiver();
            _messageReceiver.RegisterMessageHandler(MessageHandler, new MessageHandlerOptions(ErrorHandler));
        }

        private Task MessageHandler(Message message, CancellationToken cancellationToken)
        {
            TEvent @event = JsonSerializer.Deserialize<TEvent>(Encoding.UTF8.GetString(message.Body));            

            return ProcessMessageAsync(new ProcessMessageEventArgs<TEvent>() { Data = @event }, cancellationToken);
        }

        private Task ErrorHandler(ExceptionReceivedEventArgs args)
        {
            //return ProcessErrorAsync(new ProcessErrorEventArgs() { Exception = args.Exception });
            throw new MessagingConsumerException(args.Exception);
        }
    }
}
