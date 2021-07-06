using Liquid.Core.Extensions;
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
            services.AddLiquidConfiguration();
            return services;
        }

        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">The application.</param>
        public static IApplicationBuilder ConfigureApplication(this IApplicationBuilder app)
        {
            app.UseExceptionHandler();
            app.UseCultureHandler();
            return app;
        }
    }
}