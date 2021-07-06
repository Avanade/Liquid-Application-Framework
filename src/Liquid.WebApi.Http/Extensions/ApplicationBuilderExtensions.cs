using System.Diagnostics.CodeAnalysis;
using Liquid.WebApi.Http.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Liquid.WebApi.Http.Extensions
{
    /// <summary>
    /// .Net application builder extensions class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds the culture handler middleware to the application builder.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>
        /// The application builder.
        /// </returns>
        public static IApplicationBuilder UseCultureHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LiquidCultureMiddleware>();
        }

        /// <summary>
        /// Adds the custom channel handler middleware to the application builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        //public static IApplicationBuilder UseChannelHandler(this IApplicationBuilder builder)
        //{
        //    return builder.UseMiddleware<ChannelHandlerMiddleware>();
        //}

        /// <summary>
        /// Adds the exception handler middleware class to the application builder.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>
        /// Application Builder class.
        /// </returns>
        //public static IApplicationBuilder UseExceptionHandler(this IApplicationBuilder builder)
        //{
        //    return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        //}


        /// <summary>
        /// Adds the context data handler middleware to the application builder.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>
        /// The application builder.
        /// </returns>
        //public static IApplicationBuilder UseContextHandler(this IApplicationBuilder builder)
        //{
        //    return builder.UseMiddleware<ContextDataHandlerMiddleware>();
        //}
    }
}