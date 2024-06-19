using Liquid.Core.Interfaces;
using Liquid.Core.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pixel.Platform.Infra.OpenAi;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.ChatCompletions.OpenAi.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class IServiceCollectionExtension
    {

        public static IServiceCollection AddLiquidOpenAiCompletions(this IServiceCollection services, string sectionName)
        {
            services.AddOptions<GenAiOptions>()
               .Configure<IConfiguration>((settings, configuration) =>
               {
                   configuration.GetSection(sectionName).Bind(settings);
               });

            services.AddSingleton<IOpenAiClientFactory, OpenAiClientFactory>();
            services.AddTransient<ILiquidChatCompletions, OpenAiChatCompletions>();

            return services;
        }
    }
}
