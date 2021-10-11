using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Messaging.ServiceBus.Extensions.DependencyInjection;
using Liquid.Repository.Mongo.Extensions;
using Liquid.Sample.Domain.Entities;
using Liquid.Sample.Domain.Handlers.SamplePut;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                    services.AddLiquidMongoWithTelemetry<SampleEntity, int>(options => {
                        options.DatabaseName = "MySampleDb";
                        options.CollectionName = "SampleCollection";
                        options.ShardKey = "id";
                    });
                    services.AddServiceBusProducer<SampleMessageEntity>("Liquid:Messaging:ServiceBus:SampleProducer");
                    services.AddLiquidServiceBusConsumerPoc<Worker, SampleMessageEntity>("Liquid:Messaging:ServiceBus:SampleConsumer", typeof(PutCommandRequest).Assembly);
                });
    }
}
