using Liquid.Core.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Liquid.Cache.DistributedCache.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddLiquidMemoryDistributedCache(this IServiceCollection services, IConfiguration configuration,
            string section, bool withTelemetry)
        {

            services.AddDistributedMemoryCache(options => configuration.GetSection(section));

            return AddLiquidDistributedCache(services, withTelemetry);
        }

        public static IServiceCollection AddLiquidSqlServerDistributedCache(this IServiceCollection services, IConfiguration configuration,
            string section, bool withTelemetry)
        {
            services.AddDistributedSqlServerCache(options => configuration.GetSection(section));

            return AddLiquidDistributedCache(services, withTelemetry);
        }

        public static IServiceCollection AddLiquidRedisDistributedCache(this IServiceCollection services, IConfiguration configuration,
            string section, bool withTelemetry)
        {
            services.AddStackExchangeRedisCache(options => configuration.GetSection(section));

            return AddLiquidDistributedCache(services, withTelemetry);
        }

        //public static IServiceCollection AddLiquidNCacheDistributedCache(this IServiceCollection services, IConfiguration configuration,
        //    string section, bool withTelemetry)
        //{
        //    services.AddNCacheDistributedCache(options => configuration.GetSection(section));

        //    return AddLiquidDistributedCache(services, withTelemetry);
        //}

        private static IServiceCollection AddLiquidDistributedCache(IServiceCollection services, bool withTelemetry)
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