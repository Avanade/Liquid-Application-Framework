using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Messaging.ServiceBus.Extensions.DependencyInjection;
using Liquid.Repository.Mongo.Extensions;
using Liquid.Sample.Domain.Entities;
using Liquid.Sample.Domain.Handlers.SamplePut;
using Microsoft.Extensions.Hosting;

namespace Liquid.Sample.MessagingConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLiquidConfiguration();
                    services.AddLiquidMongoRepository<SampleEntity, int>("Liquid:MyMongoDbSettings:Entities");
                    services.AddLiquidServiceBusProducer<SampleMessageEntity>("Liquid:Messaging:ServiceBus:SampleProducer");
                    services.AddLiquidServiceBusConsumer<Worker, SampleMessageEntity>("Liquid:Messaging:ServiceBus:SampleConsumer", true, typeof(PutCommandRequest).Assembly);
                });
    }
}
