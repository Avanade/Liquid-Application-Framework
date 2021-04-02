using Liquid.Cache.Redis.Configuration;
using Liquid.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Liquid.Cache.Redis
{
    /// <summary>
    /// Redis Cache Extensions.
    /// </summary>
    public static class LightRedisCacheExtension
    {
        /// <summary>
        /// Adds the redis cache.
        /// </summary>
        /// <param name="services">The services.</param>
        public static IServiceCollection AddLightRedisCache(this IServiceCollection services)
        {
            services.AddSingleton<ILightCache, LightRedisCache>();
            services.AddSingleton<ILightConfiguration<RedisCacheSettings>, LightRedisCacheConfiguration>();
            return services;
        }
    }
}