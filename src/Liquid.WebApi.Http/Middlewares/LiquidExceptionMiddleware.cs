using FluentValidation;
using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Liquid.WebApi.Http.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;

namespace Liquid.WebApi.Http.Middlewares
{
    /// <summary>
    /// Generates a log and serialize a formated Json response object for generic exceptions.
    /// Includes its behavior in netcore pipelines after request execution when thows error.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LiquidExceptionMiddleware
    {
        private readonly ILogger<LiquidExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly ILiquidSerializerProvider _serializerProvider;

        /// <summary>
        /// Initialize a new instance of <see cref="LiquidExceptionMiddleware"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serializerProvider"></param>
        /// <param name="next"></param>
        public LiquidExceptionMiddleware(RequestDelegate next
            , ILogger<LiquidExceptionMiddleware> logger
            , ILiquidSerializerProvider serializerProvider)
        {
            _logger = logger;
            _next = next;
            _serializerProvider = serializerProvider;
        }

        /// <summary>
        /// Generates a log and serialize a formated Json response object for exceptions.
        /// Includes its behavior in netcore pipelines after request execution when thows error.
        /// </summary>
        /// <param name="context">HTTP-specific information about an individual HTTP request.</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, $"Liquid request validation error: {ex}");
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error: {ex}");
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
        {
            context.Response.ContentType = context.Request.ContentType;

            context.Response.StatusCode = (int)statusCode;

            var response = new LiquidErrorResponse()
            {
                StatusCode = context.Response.StatusCode,
                Message = "An error occurred whilst processing your request.",
                Detailed = exception
            };

            var serializer = GetSerializer(context.Request.ContentType);

            return context.Response.WriteAsync(serializer.Serialize(response));
        }

        private ILiquidSerializer GetSerializer(string contentType)
        {
            ILiquidSerializer serializer;

            switch (contentType)
            {
                case "application/json":
                    serializer = _serializerProvider.GetSerializerByType(typeof(LiquidJsonSerializer));
                    break;
                case "application/xml":
                    serializer = _serializerProvider.GetSerializerByType(typeof(LiquidXmlSerializer));
                    break;
                default:
                    serializer = _serializerProvider.GetSerializerByType(typeof(LiquidJsonSerializer));
                    break;
            }

            return serializer;
        }
    }
}
