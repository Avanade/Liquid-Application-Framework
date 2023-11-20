using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xrm.Sdk;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Adapter.Dataverse.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods of <see cref="IServiceCollection"/>
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers <see cref="DataverseAdapter"/> service, it's dependency
        /// <see cref="DataverseClientFactory"/>, and also set configuration
        /// option <see cref="DataverseSettings"/>.
        /// Also register <see cref="DataverseEntityMapper"/> service.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dataverseSection">configuration section of dataverse settings.</param>
        public static IServiceCollection AddLiquidDataverseAdapter(this IServiceCollection services, string dataverseSection)
        {
            services.AddOptions<DataverseSettings>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection(dataverseSection).Bind(settings);
            });

            services.AddTransient<IDataverseClientFactory, DataverseClientFactory>();

            services.AddSingleton<ILiquidDataverseAdapter, DataverseAdapter>();

            services.AddSingleton<ILiquidMapper<string, Entity>, DataverseEntityMapper>();

            return services;
        }
    }
}
