using Liquid.Adapter.Dataverse.Extensions.DependencyInjection;
using Liquid.Sample.Dataverse.Function;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Liquid.Sample.Dataverse.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLiquidDataverseAdapter("DataverseClient");
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();

            builder.ConfigurationBuilder
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, $"local.setttings.json"), optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }
    }
}
