using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Core.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Liquid.Cache.Extensions.DependencyInjection
{
    /// <summary>
    /// LiquidCache <see cref="IServiceCollection"/> extensions class.
    /// </summary>
    public static class IServiceCollectionExtension
    {
        /// <summary>
        /// Registers a <see cref="LiquidCache"/> service and its <see cref="LiquidTelemetryInterceptor"/> 
        /// depending on the value of <paramref name="withTelemetry"/>.
        /// </summary>
        /// <param name="services">Extended IServiceCollection.</param>
        /// <param name="withTelemetry">indicates if this method must register a <see cref="LiquidTelemetryInterceptor"/></param>
        /// <returns></returns>
        public static IServiceCollection AddLiquidDistributedCache(this IServiceCollection services, bool withTelemetry)
        {
            if (withTelemetry)
            {
                services.AddScoped<LiquidCache>();
                services.AddScopedLiquidTelemetry<ILiquidCache, LiquidCache>();
            }
            else
                services.AddScoped<ILiquidCache, LiquidCache>();

            return services;
        }

    }
}