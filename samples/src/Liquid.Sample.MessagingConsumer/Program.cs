using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Messaging.ServiceBus.Extensions.DependencyInjection;
using Liquid.Repository.Mongo.Extensions;
using Liquid.Sample.Domain.Entities;
using Liquid.Sample.Domain.Handlers.SamplePut;
using Microsoft.Extensions.Hosting;
using System;

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
                    services.AddLiquidMongoRepository<SampleEntity, Guid>("Liquid:MyMongoDbSettings:Entities");
                    services.AddLiquidServiceBusProducer<SampleMessageEntity>("ServiceBus", "liquidoutput", false);
                    services.AddLiquidServiceBusConsumer<Worker, SampleMessageEntity> ("ServiceBus", "liquidinput", false, typeof(PutCommandRequest).Assembly);
                });
    }
}
