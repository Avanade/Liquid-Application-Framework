using Alachisoft.NCache.Caching.Distributed;
using Alachisoft.NCache.Caching.Distributed.Configuration;
using Liquid.Cache.Extensions.DependencyInjection;
using Liquid.Core.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Liquid.Cache.NCache.Extensions.DependencyInjection
{
    /// <summary>
    /// LiquidCache using NCache <see cref="IServiceCollection"/> extensions class.
    /// </summary>
    public static class IServiceCollectionExtension
    {
        /// <summary>
        /// Registers <see cref="NCacheDistributedCache"/> service and <see cref="LiquidCache"/> decorator,
        /// with its <see cref="LiquidTelemetryInterceptor"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction">An System.Action`1 to configure the provided
        /// <see cref="NCacheConfiguration"/>.</param>
        /// <param name="withTelemetry">Indicates if this method must register a <see cref="LiquidTelemetryInterceptor"/></param>
        public static IServiceCollection AddLiquidNCacheDistributedCache(this IServiceCollection services,
             Action<NCacheConfiguration> setupAction, bool withTelemetry = true)
        {
            services.AddNCacheDistributedCache(setupAction);

            return services.AddLiquidDistributedCache(withTelemetry);
        }
    }
}
