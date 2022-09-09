using Liquid.Cache.Extensions.DependencyInjection;
using Liquid.Core.Implementations;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Liquid.Cache.SqlServer.Extensions.DependencyInjection
{
    /// <summary>
    /// LiquidCache using SqlServer <see cref="IServiceCollection"/> extensions class.
    /// </summary>
    public static class IServiceCollectionExtension
    {
        /// <summary>
        /// Registers <see cref="SqlServerCache"/> service and <see cref="LiquidCache"/> decorator,
        /// with its <see cref="LiquidTelemetryInterceptor"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction">An System.Action`1 to configure the provided
        ///<see cref="SqlServerCacheOptions"/>.</param>
        /// <param name="withTelemetry">Indicates if this method must register a <see cref="LiquidTelemetryInterceptor"/></param>
        public static IServiceCollection AddLiquidSqlServerDistributedCache(this IServiceCollection services,
            Action<SqlServerCacheOptions> setupAction, bool withTelemetry = true)
        {
            services.AddDistributedSqlServerCache(setupAction);

            return services.AddLiquidDistributedCache(withTelemetry);
        }
    }
}
