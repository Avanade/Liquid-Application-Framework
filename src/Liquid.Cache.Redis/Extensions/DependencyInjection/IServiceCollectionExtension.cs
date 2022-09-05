using Liquid.Cache.DistributedCache.Extensions.DependencyInjection;
using Liquid.Core.Implementations;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Liquid.Cache.Redis.Extensions.DependencyInjection
{
    /// <summary>
    /// LiquidCache using Redis <see cref="IServiceCollection"/> extensions class.
    /// </summary>
    public static class IServiceCollectionExtension
    {
        /// <summary>
        /// Registers <see cref="RedisCache"/> service and <see cref="LiquidCache"/> decorator,
        /// with its <see cref="LiquidTelemetryInterceptor"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction">An System.Action`1 to configure the provided
        /// Microsoft.Extensions.Caching.StackExchangeRedis.RedisCacheOptions.</param>
        /// <param name="withTelemetry">Indicates if this method must register a <see cref="LiquidTelemetryInterceptor"/></param>
        public static IServiceCollection AddLiquidRedisDistributedCache(this IServiceCollection services,
            Action<RedisCacheOptions> setupAction, bool withTelemetry = true)
        {
            services.AddStackExchangeRedisCache(setupAction);

            return services.AddLiquidDistributedCache(withTelemetry);
        }
    }
}
