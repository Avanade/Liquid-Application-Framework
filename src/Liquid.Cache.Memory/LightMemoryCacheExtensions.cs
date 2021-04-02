using Microsoft.Extensions.DependencyInjection;

namespace Liquid.Cache.Memory
{
    /// <summary>
    /// Memory Cache Extensions.
    /// </summary>
    public static class LightMemoryCacheExtensions
    {
        /// <summary>
        /// Adds the cache memory.
        /// </summary>
        /// <param name="services">The services.</param>
        public static IServiceCollection AddLightMemoryCache(this IServiceCollection services)
        {
            services.AddSingleton<ILightCache, LightMemoryCache>();
            return services;
        }
    }
}