using Liquid.Core.GenAi;
using Liquid.GenAi.OpenAi.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.GenAi.OpenAi.Extensions
{
    /// <summary>
    /// Extension methods to register Liquid OpenAi Completions services.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class IServiceCollectionExtension
    {
        /// <summary>
        /// extension method to register Liquid OpenAi Completions service,
        /// factory and binding settings from configuration.
        /// </summary>
        /// <param name="services">extension method reference</param>
        /// <param name="sectionName">configuration section name</param> 
        public static IServiceCollection AddLiquidOpenAiCompletions(this IServiceCollection services, string sectionName)
        {
            services.AddOptions<OpenAiOptions>()
               .Configure<IConfiguration>((settings, configuration) =>
               {
                   configuration.GetSection(sectionName).Bind(settings);
               });

            services.AddSingleton<IOpenAiClientFactory, OpenAiClientFactory>();
            services.AddTransient<ILiquidGenAi, OpenAiAdapter>();

            return services;
        }
    }
}
