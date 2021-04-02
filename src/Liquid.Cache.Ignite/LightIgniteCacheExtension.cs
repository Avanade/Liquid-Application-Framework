using Liquid.Cache.Ignite.Configuration;
using Liquid.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Liquid.Cache.Ignite
{
    /// <summary>
    /// Apache Ignite Extensions.
    /// </summary>
    public static class LightIgniteCacheExtension
    {
        /// <summary>
        /// Adds the Apache Ignite cache to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        public static IServiceCollection AddLightIgniteCache(this IServiceCollection services)
        {
            services.AddSingleton<ILightCache, LightIgniteCache>();
            services.AddSingleton<ILightConfiguration<IgniteCacheSettings>, LightIgniteCacheConfiguration>();
            return services;
        }
    }
}