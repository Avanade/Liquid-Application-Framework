using Liquid.Core.Configuration;
using Liquid.Core.Context;
using Liquid.Core.Localization;
using Liquid.Core.Telemetry;
using Liquid.WebApi.Http.Factories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Http.Extensions
{
    /// <summary>
    /// Startup extension methods. Used to configure the startup application.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class StartupExtensions
    {
        /// <summary>
        /// Adds the web API services.
        /// </summary>
        /// <param name="services">The services.</param>
        public static IServiceCollection ConfigureLiquidHttp(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddLocalizationService();

            services.AddConfigurations(typeof(StartupExtensions).Assembly);
            services.AddScoped<ILightContext, LightContext>();
            services.AddTransient<ILightContextFactory, WebApiLightContextFactory>();

            services.AddScoped<ILightTelemetry, LightTelemetry>();
            services.AddSingleton<ILightTelemetryFactory, WebApiLightTelemetryFactory>();
            return services;
        }

        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">The application.</param>
        public static IApplicationBuilder ConfigureApplication(this IApplicationBuilder app)
        {
            app.UseContextHandler();
            app.UseTelemetry();
            app.UseExceptionHandler();
            app.UseCultureHandler();
            app.UseChannelHandler();

            return app;
        }        
    }
}