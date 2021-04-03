using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using Liquid.Core.Exceptions;
using Liquid.Core.Utils;
using Liquid.Core.Context;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Grpc.Core.Interceptors;
using Grpc.Core;

namespace Liquid.WebApi.Grpc.Interceptors
{
    /// <summary>
    /// Global Exception Handler Middleware. 
    /// Responsible for handling all unexpected exceptions that may occur in each request.
    /// </summary>
    /// <seealso cref="ExceptionFilterAttribute" />
    [ExcludeFromCodeCoverage]
    internal sealed class ExceptionHandlerInterceptor : Interceptor
    {
        private readonly ILightContextFactory _contextFactory;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlerInterceptor" /> class
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <exception cref="ArgumentNullException">next</exception>
        public ExceptionHandlerInterceptor(ILightContextFactory contextFactory, ILoggerFactory loggerFactory)
        {
            _contextFactory = contextFactory;
            _logger = loggerFactory.CreateLogger<ExceptionHandlerInterceptor>();
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
                                                                                      ServerCallContext context,
                                                                                      UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (Exception ex)
            {
                var exception = ex;
                if (ex is AggregateException aggregateException) exception = aggregateException.Flatten();
                var customContext = _contextFactory.GetContext();
                var correlation = customContext.ContextId.ToString();
                var timestamp = DateTime.Now;
                IDictionary<string, object> customData = new Dictionary<string, object>();
                if (exception is LightException)
                {
                    var customProperties = exception.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
                    customProperties.Each(property => customData.Add($"customData_{property.Name}", property.GetValue(exception)));
                }
                exception.Data.Add("correlation", correlation);
                var resultObject =
                    new
                    {
                        timestamp,
                        correlation,
                        exception.Message,
                        customData,
                        innerException = exception.InnerException?.ToString(),
                        exception.StackTrace
                    }.ToJson(new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, });

#pragma warning disable 4014
                Task.Run(() => _logger.LogError(exception, resultObject));
#pragma warning restore 4014

                throw;
            }
        }

    }
}