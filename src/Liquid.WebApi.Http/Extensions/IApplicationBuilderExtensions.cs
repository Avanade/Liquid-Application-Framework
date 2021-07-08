using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Interfaces;
using Liquid.WebApi.Http.Middlewares;
using Liquid.WebApi.Http.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Liquid.WebApi.Http.Extensions
{
    /// <summary>
    /// .Net application builder extensions class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds <see cref="LiquidCultureMiddleware"/> to the application builder.
        /// </summary>
        /// <param name="builder">Extended application builder.</param>
        public static IApplicationBuilder UseLiquidCulture(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LiquidCultureMiddleware>();
        }

        /// <summary>
        /// Adds <see cref="LiquidExceptionMiddleware"/> to the application builder.
        /// </summary>
        /// <param name="builder">Extended application builder.</param>
        public static IApplicationBuilder UseLiquidException(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LiquidExceptionMiddleware>();
        }

        /// <summary>
        /// Adds <see cref="LiquidContextMiddleware"/> to the application builder.
        /// </summary>
        /// <param name="builder">Extended application builder.</param>
        public static IApplicationBuilder UseLiquidContext(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LiquidContextMiddleware>();
        }

        /// <summary>
        /// Register the Swagger middleware with Liquid Configuration settings.
        /// </summary>
        /// <param name="builder">Extended application builder.</param>
        public static IApplicationBuilder UseLiquidSwagger(this IApplicationBuilder builder)
        {
            var configuration = builder.ApplicationServices.GetService<ILiquidConfiguration<SwaggerSettings>>();

            var swaggerSettings = configuration.Settings;
            builder.UseSwagger().UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(swaggerSettings.SwaggerEndpoint.Url, swaggerSettings.SwaggerEndpoint.Name);
            });
            return builder;
        }

        /// <summary>
        /// Adds <see cref="LiquidCultureMiddleware"/> , <see cref="LiquidContextMiddleware"/> and
        /// <see cref="LiquidExceptionMiddleware"/> to the application builder in this order, 
        /// and finally register the Swagger middleware with Liquid Configuration settings.
        /// </summary>
        /// <param name="builder">Extended application builder.</param>
        public static IApplicationBuilder UseLiquidConfigure(this IApplicationBuilder builder)
        {

            builder.UseLiquidCulture();
            builder.UseLiquidContext();
            builder.UseLiquidSwagger();
            builder.UseLiquidException();

            return builder;
        }
    }
}