using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Core.Implementations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Liquid.Cache.Memory.Extensions.DependencyInjection
{
    /// <summary>
    /// LiquidCache using Redis <see cref="IServiceCollection"/> extensions class.
    /// </summary>
    public static class IServiceCollectionExtension
    {
        /// <summary>
        /// Registers <see cref="MemoryDistributedCache"/> service and <see cref="LiquidCache"/> decorator,
        /// with its <see cref="LiquidTelemetryInterceptor"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction">An System.Action`1 to configure the provided 
        /// <see cref="MemoryDistributedCacheOptions"/>.</param>
        /// <param name="withTelemetry">Indicates if this method must register a <see cref="LiquidTelemetryInterceptor"/></param>
        public static IServiceCollection AddLiquidMemoryDistributedCache(this IServiceCollection services,
            Action<MemoryDistributedCacheOptions> setupAction, bool withTelemetry = true)
        {
            services.AddDistributedMemoryCache(setupAction);

            return services.AddLiquidDistributedCache(withTelemetry);
        }
    }
}
