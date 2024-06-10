using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Adapter.AzureStorage.Extensions
{
    /// <summary>
    /// Extension methods of <see cref="IServiceCollection"/>
    /// for register Liquid Azure Storage services.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers <see cref="BlobStorageAdapter"/> service, it's dependency
        /// <see cref="BlobClientFactory"/>, and also set configuration
        /// option <see cref="StorageSettings"/>.
        /// </summary>
        /// <param name="services">service collection instance.</param>
        /// <param name="configSection">configuration section of storage settings.</param>
        public static IServiceCollection AddLiquidAzureStorageAdapter(this IServiceCollection services, string configSection)
        {
            services.AddOptions<StorageSettings>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection(configSection).Bind(settings);
            });

            services.AddSingleton<IBlobClientFactory, BlobClientFactory>();

            services.AddScoped<ILiquidBlobStorageAdapter, BlobStorageAdapter>();

            return services;
        }
    }
}
