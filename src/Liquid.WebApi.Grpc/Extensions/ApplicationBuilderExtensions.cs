using System.Diagnostics.CodeAnalysis;
using Liquid.WebApi.Grpc.Interceptors;
using Microsoft.AspNetCore.Builder;

namespace Liquid.WebApi.Grpc.Extensions
{
    /// <summary>
    /// .Net application builder extensions class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds the telemetry middleware to the application builder.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>
        /// The application builder.
        /// </returns>
        public static IApplicationBuilder UseTelemetry(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TelemetryHandlerMiddleware>();
        }
    }
}