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

        private readonly ILiquidMessagingPipeline<Message> _pipeline;

        public event Func<ProcessMessageEventArgs<TEvent>, CancellationToken, Task> ProcessMessageAsync;
        public event Func<ProcessErrorEventArgs, Task> ProcessErrorAsync;

        public ServiceBusConsumer(IServiceBusFactory factory, ILiquidMessagingPipeline<Message> pipeline)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _pipeline = pipeline;
        }
        public void Start()
        {
            _messageReceiver = _factory.GetReceiver();
            _messageReceiver.RegisterMessageHandler(MessageHandler, new MessageHandlerOptions(ErrorHandler));
        }

        private async Task MessageHandler(Message message, CancellationToken cancellationToken)
        {   
            var responseMessage = _pipeline.ExecutePreProcessor(message);

            TEvent @event = JsonSerializer.Deserialize<TEvent>(Encoding.UTF8.GetString(responseMessage.Body));

            await _pipeline.ExecutePostProcessor<TEvent>(ProcessMessageAsync(new ProcessMessageEventArgs<TEvent>() { Data = @event }, cancellationToken));
        }

        private Task ErrorHandler(ExceptionReceivedEventArgs args)
        {
            return ProcessErrorAsync(new ProcessErrorEventArgs() { 
                Exception = new MessagingConsumerException(args.Exception) 
            });
        }
    }
}
