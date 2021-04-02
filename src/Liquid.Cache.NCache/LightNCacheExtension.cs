using Liquid.Cache.NCache.Configuration;
using Liquid.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Liquid.Cache.NCache
{
    /// <summary>
    /// NCache Extensions.
    /// </summary>
    public static class LightNCacheExtension
    {
        /// <summary>
        /// Adds the NCache cache to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        public static IServiceCollection AddLightNCache(this IServiceCollection services)
        {
            services.AddSingleton<ILightCache, LightNCache>();
            services.AddSingleton<ILightConfiguration<NCacheSettings>, LightNCacheConfiguration>();
            return services;
        }
    }
}