using Liquid.Core.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PROJECTNAME.Domain.Entities;
using PROJECTNAME.Domain.Handlers;
using System;

namespace PROJECTNAME.WorkerService
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
                    //TODO: Register and configure messaging consumer and repository Liquid Cartridge
                    //
                    // Example:
                    //
                    // [ServiceBus and Mongo Cartridges]
                    // 1. add Liquid Cartridge using CLI : dotnet add package Liquid.Messaging.ServiceBus --version 6.X.X
                    // 2. add Liquid Cartridge using CLI : dotnet add package Liquid.Repository.Mongo --version 6.X.X
                    // 3. import liquid cartridge reference here: using Liquid.Repository.Mongo.Extensions;
                    // 4. import liquid cartridge reference here: using Liquid.Messaging.ServiceBus.Extensions.DependencyInjection;
                    // 5. call repository cartridge DI method :
                    //  services.AddLiquidMongoRepository<ENTITYNAME, Guid>("Liquid:MyMongoDbSettings", "SampleCollection");
                    // 6. call messaging cartridge DI method :
                    //  services.AddLiquidServiceBusConsumer<Worker, ENTITYNAME>("Liquid:ServiceBus", "liquidinput", false, typeof(COMMANDNAMEENTITYNAMERequest).Assembly);
                    // 7. edit appsettings.json file to include database and message queue configurations.

                });
    }
}
