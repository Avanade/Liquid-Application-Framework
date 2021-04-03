| [Main](About-Liquid.md) > [Key Concepts](Key-Concepts.md) > Business Logic Separation |
|----|

For the sake of productivity, is better to have source code clear and legible, as much as possible.

However, current technology platforms contain many components to be integrated and too many aspects to consider beyond the (business) algorithm in question.

It is hard to deal with those technology matters in a isolated way, seggregated from the core business logic that is being written.

Liquid seeks to do that, as good as possible, allowing to use only minimum and precise mentions to such technical capabilities in the business code.

Take the following example showing how Liquid hides most of the complexity of connecting to a data repository to find and update some data and, in the same logic, publishing an event in a queue of a message/event bus:

On Startup.cs:
```csharp
...
    //Injects the Messaging Service
    services.AddProducersConsumers(typeof(ProductPriceChangedPublisher).Assembly);
...
```
On ChangeProductPriceCommandHandler.cs:
```csharp
    public class ChangeProductPriceCommandHandler : RequestHandlerBase, IRequestHandler<ChangeProductPriceCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILightProducer<ProductPriceChangedMessage> _productPriceChangedPublisher;
        private readonly IMapper _mapper;
        private readonly IPostService _postService;

        public ChangeProductPriceCommandHandler(IMediator mediatorService, 
                                                ILightContext contextService, 
                                                ILightTelemetry telemetryService, 
                                                IMapper mapperService,
                                                IProductRepository productRepository,
                                                ILightProducer<ProductPriceChangedMessage> productPriceChangedPublisher,
                                                IPostService postService) : base(mediatorService, contextService, telemetryService, mapperService)
        {
        }




        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<bool> Handle(ChangeProductPriceCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FindByIdAsync(new ObjectId(request.Id));
            if (product != null)
            {
                product.Price = request.Price;
                await _productRepository.UpdateAsync(product);
                var changeProductPriceMessage = _mapper.Map<ProductPriceChangedMessage>(request);
                await _productPriceChangedPublisher.SendMessageAsync(changeProductPriceMessage);
                return true;
            }
            throw new ProductDoesNotExistException();
        }
    }
```

The method `public async Task<bool> Handle(...` above is responsible for doing the business logic. It should find Product data in a repository, change its price, persist it back to the repository and publish an event. The classes `_productRepository` and `_productPriceChangedPublisher` hides the technical complexity mentioned before. Besides that, since those classes are injected on this Command Handler, they can be substituted by any other classes implementing the same interfaces (`IProductRepository` and `ILightProducer<ProductPriceChangedMessage>` respectivelly), doing a complete isolation of the business logic and the underlying infrastructure components.

In this case, Liquid uses a common programming model in DDD hexagonal architectures, where external requests will be translated to Domain Commands or Domain Queries, and those will handle the business logic that responds to those requests. In that way, the requests can came from anywhere and in any format that could be handled outside the Domain. The same for the reponses.

This strategy is followed for all technology capabilities that Liquid abstracts and pre-integrates. 
For instance, see bellow, how simple is to create the class to publish that event on a RAbbitMQ:
```csharp
    [RabbitMqProducer("rabbitMqConnectionString", "ProductPriceChanged")]
    public class ProductPriceChangedPublisher : RabbitMqProducer<ProductPriceChangedMessage>
    {
        /// <summary>
        /// Gets the rabbit mq settings.
        /// </summary>
        /// <value>
        /// The rabbit mq settings.
        /// </value>
        public override RabbitMqSettings RabbitMqSettings => new RabbitMqSettings
        {
            Durable = true,
            Persistent = true,
            Expiration = "172800000"
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductPriceChangedPublisher"/> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingSettings">The messaging settings.</param>
        public ProductPriceChangedPublisher(ILightContextFactory contextFactory, 
                                            ILightTelemetryFactory telemetryFactory, 
                                            ILoggerFactory loggerFactory, 
                                            ILightConfiguration<List<MessagingSettings>> messagingSettings) : base(contextFactory, telemetryFactory, loggerFactory, messagingSettings)
        {
        }
    }
```
The base class `RabbitMqProducer` will handle all the complexities inherent to connect to a RabbitMQ server and publish a message on a queue.
That is the beauty of using platform specific "cartridges". This way, it should be very easy to develop an application that could run on an on-premises environment using RabbitMQ as the message bus, and, also, the same application could be changed to run on Azure, using Azure ServiceBus as the message bus. In fact, the application could be created to dinamically switch between different platform implementations, in run time.

In contrast to what Liquid provides, see an example excerpt from the code of [.NET Core reference application](https://github.com/dotnet-architecture/eShopOnContainers/tree/dev/src/Services/Ordering/Ordering.BackgroundTasks):

```
...

private void RegisterEventBus(IServiceCollection services)
        {
            var subscriptionClientName = Configuration["SubscriptionClientName"];

            if (Configuration.GetValue<bool>("AzureServiceBusEnabled"))
            {
                services.AddSingleton<IEventBus, EventBusServiceBus>(sp =>
                {
                    var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                    var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                    var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
                    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                    return new EventBusServiceBus(serviceBusPersisterConnection, logger,
                        eventBusSubcriptionsManager, subscriptionClientName, iLifetimeScope);
                });
            }

            ...

            RegisterEventBus(services);

            //create autofac based service provider
            var container = new ContainerBuilder();
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());
}

...

 private void ConfigureEventBus(IApplicationBuilder app)

        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            
            ...

            eventBus.Subscribe<OrderStatusChangedToShippedIntegrationEvent, OrderStatusChangedToShippedIntegrationEventHandler>();
        }

...

    public class OrderStatusChangedToShippedIntegrationEventHandler : IIntegrationEventHandler<OrderStatusChangedToShippedIntegrationEvent>
    {
        private readonly IHubContext<NotificationsHub> _hubContext;

        public OrderStatusChangedToShippedIntegrationEventHandler(IHubContext<NotificationsHub> hubContext)
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        }

        public async Task Handle(OrderStatusChangedToShippedIntegrationEvent @event)
        {
        ... 
        }

```
There are too many technical (platform specific) lines of code mixed with only a few application (business) ones:  **OrderStatusChangedToShippedIntegrationEvent** and corresponding **Handle()**.

In this scenario, it is very hard to understand, refactor and evolve this code due to both technical and business demands.

With Liquid, the business logic will always be inside the domain, seggregated from the underlying technology, making it easier to understand. At same time, the underlying technology specifics will be outside the domain, abstracted in a way that makes possible to use different implementations that will work with the same business logic, while, also, allows to evolve these parts of code separatelly.
