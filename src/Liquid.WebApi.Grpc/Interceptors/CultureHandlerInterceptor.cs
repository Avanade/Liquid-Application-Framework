using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Liquid.Core.Configuration;
using Liquid.Core.Context;
using Liquid.Core.Localization;
using Liquid.WebApi.Grpc.Extensions;

namespace Liquid.WebApi.Grpc.Interceptors
{
    /// <summary>
    /// Handles the culture code information from request. Checks the culture code either from header or querystring.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class CultureHandlerInterceptor : Interceptor
    {
        private const string CultureTag = "culture";
        private readonly ILightContextFactory _contextFactory;
        private readonly ILightConfiguration<CultureSettings> _cultureSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureHandlerInterceptor" /> class.
        /// </summary>
        /// <param name="contextFactory">The service provider.</param>
        /// <param name="cultureSettings">The culture settings.</param>
        public CultureHandlerInterceptor(ILightContextFactory contextFactory, ILightConfiguration<CultureSettings> cultureSettings)
        {
            _contextFactory = contextFactory;
            _cultureSettings = cultureSettings;
        }

        /// <summary>
        /// Server-side handler for intercepting and incoming unary call.
        /// </summary>
        /// <typeparam name="TRequest">Request message type for this method.</typeparam>
        /// <typeparam name="TResponse">Response message type for this method.</typeparam>
        /// <param name="request">The request value of the incoming invocation.</param>
        /// <param name="context">An instance of <see cref="T:Grpc.Core.ServerCallContext" /> representing
        /// the context of the invocation.</param>
        /// <param name="continuation">A delegate that asynchronously proceeds with the invocation, calling
        /// the next interceptor in the chain, or the service request handler,
        /// in case of the last interceptor and return the response value of
        /// the RPC. The interceptor can choose to call it zero or more times
        /// at its discretion.</param>
        /// <returns>
        /// A future representing the response value of the RPC. The interceptor
        /// can simply return the return value from the continuation intact,
        /// or an arbitrary response value as it sees fit.
        /// </returns>
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            HandleCulture(context);
            return await continuation(request, context);
        }

        /// <summary>
        /// Handles the culture.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        private void HandleCulture(ServerCallContext context)
        {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            var cultureCode = context.GetHeaderValueFromRequest(CultureTag);
            if (string.IsNullOrEmpty(cultureCode) && !string.IsNullOrEmpty(_cultureSettings.Settings.DefaultCulture))
            {
                cultureCode = _cultureSettings.Settings.DefaultCulture;
            }

            if (!string.IsNullOrEmpty(cultureCode))
            {
                try
                {
                    _contextFactory.GetContext().SetCulture(cultureCode);
                }
                catch
                {
                    // ignored. Left intentionally blank.
                }
            }
        }
    }
}