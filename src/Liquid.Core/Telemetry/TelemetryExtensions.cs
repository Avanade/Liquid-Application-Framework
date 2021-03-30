using Microsoft.Extensions.DependencyInjection;

namespace Liquid.Core.Telemetry
{
    /// <summary>
    /// Telemetry Extensions Class.
    /// </summary>
    public static class TelemetryExtensions
    {
        /// <summary>
        /// Adds the default telemetry.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultTelemetry(this IServiceCollection services)
        {
            services.AddScoped<ILightTelemetry, LightTelemetry>();
            services.AddSingleton<ILightTelemetryFactory, LightTelemetryFactory>();
            return services;
        }
    }
}