using Microsoft.Extensions.DependencyInjection;

namespace Liquid.Core.Context
{
    /// <summary>
    /// Context Extensions Class.
    /// </summary>
    public static class ContextExtensions
    {
        /// <summary>
        /// Adds the default context.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultContext(this IServiceCollection services)
        {
            services.AddScoped<ILightContext, LightContext>();
            services.AddSingleton<ILightContextFactory, LightContextFactory>();
            return services;
        }
    }
}