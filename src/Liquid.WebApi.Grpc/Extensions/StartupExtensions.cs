using Grpc.AspNetCore.Server;
using Liquid.Core.Configuration;
using Liquid.Core.Context;
using Liquid.Core.Localization;
using Liquid.Core.Telemetry;
using Liquid.WebApi.Grpc.Configuration;
using Liquid.WebApi.Grpc.Factories;
using Liquid.WebApi.Grpc.Interceptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Grpc.Extensions
{
    /// <summary>
    /// Startup extension methods. Used to configure the startup application.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class StartupExtensions
    {
        private static IServiceCollection _services;

        /// <summary>
        /// Adds the web API services.
        /// </summary>
        /// <param name="services">The services.</param>
        public static IServiceCollection ConfigureLiquidGrpc(this IServiceCollection services)
        {
            _services = services;
            services.AddHttpClient();
            services.AddLocalizationService();

            services.AddConfigurations(typeof(StartupExtensions).Assembly);
            services.AddScoped<ILightContext, LightContext>();
            services.AddTransient<ILightContextFactory, GrpcLightContextFactory>();

            services.AddScoped<ILightTelemetry, LightTelemetry>();
            services.AddScoped<ILightGrpcService, LightGrpcService>();
            services.AddSingleton<ILightTelemetryFactory, GrpcLightTelemetryFactory>();
            return services;
        }

        /// <summary>
        /// Configures the Liquid options in GRPC.
        /// </summary>
        /// <param name="options">The grpc options.</param>
        /// <returns></returns>
        public static GrpcServiceOptions UseLiquid(this GrpcServiceOptions options)
        {
            var serviceProvider = _services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<ILightConfiguration<ApiSettings>>();

            options.EnableDetailedErrors = configuration.Settings.ShowDetailedException;
            options.Interceptors.Add<ContextDataHandlerInterceptor>();
            options.Interceptors.Add<ExceptionHandlerInterceptor>();
            options.Interceptors.Add<CultureHandlerInterceptor>();
            options.Interceptors.Add<ChannelHandlerInterceptor>();

            return options;
        }

        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">The application.</param>
        public static IApplicationBuilder ConfigureApplication(this IApplicationBuilder app)
        {
            app.UseTelemetry();
            return app;
        }
    }
}
