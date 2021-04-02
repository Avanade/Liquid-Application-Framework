| [Main](About-Liquid.md) > [Key Concepts](Key-Concepts.md) > Platform Abstraction Layer |
|----|

The most fundamental strategy steering Liquid's design is its responsibility to decouple as much as possible the application components from the underlying platforms.

 ![PlatformAbstractionLayer.png](PlatformAbstractionLayer.png =1000x) 

Liquid concentrates most of platform (technology) specific dependencies in one single point of code thus freeing up the many application (business) components to access pre-integrated, high abstraction primitives for doing their jobs.

Doing such technology integration repetitive tasks, while error-prone and risky, can considerably increase costs on software projects. 

Hence an abstraction layer like Liquid diminishes such costs as much as great is the number of business components written on top of it - without having to deal with those low levels details.

Additionally (and even more important), now there are only a handful of dependency points on a given set of technology components (_i.e._ on their SDKs and APIs).

Following the Liquid guiding principle of [multi-platform components](Introduction.md/#guiding-principles), the pretty easy [replacement of one specific Liquid cartridge by another](Choose-platform-components.md) allows the avoidance of technology lock-in in a controlled and productive way.

See parts of a sample code that activates RabbitMQ as the message bus for asynchronous communication:

On Startup.cs:
```csharp
    //Injects the Messaging Service
    services.AddProducersConsumers(typeof(ProductPriceChangedPublisher).Assembly);
```

On ProductPriceChangedPublisher.cs:
```csharp
    [RabbitMqProducer("messageBusConnectionString", "ProductPriceChanged")]
    public class ProductPriceChangedPublisher : RabbitMqProducer<ProductPriceChangedMessage>
```

While you can easily change this code to use Azure ServiceBus, for sample, this approach makes possible to [use the best of breed from all platforms](Leveling-up-Platform-Providers.md), because you are able to implement specific technical functionalities of each platform on the specific cartridge (RabbitMqProducer in this case). Your point of maintenance in the code for such kind of change will not touch any business logic and will be very easy to do.
For use Azure ServiceBus instead of RabbitMQ, the only change would be on ProductPriceChangedPublisher.cs, as follows:
```csharp
    [ServiceBusProducer("messageBusConnectionString", "ProductPriceChanged")]
    public class ProductPriceChangedPublisher : ServiceBusProducer<ProductPriceChangedMessage>
```

Actually, the corollary of a given market standard is to be a commodity technology.

Accordingly, Liquid helps to transcend the classical trade-off between being open & commodity or being locked-in & differentiated.

Adding new technology platform capabilities to all business application components can be done, once again, from a centralized point, in a controlled and agile way.

