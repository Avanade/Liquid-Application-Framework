using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Liquid.Core.Context;
using Liquid.WebApi.Grpc.Extensions;

namespace Liquid.WebApi.Grpc.Interceptors
{
    /// <summary>
    /// Channel Handler Interceptor class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ChannelHandlerInterceptor : Interceptor
    {
        private const string ChannelTag = "channel";
        private readonly ILightContextFactory _contextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelHandlerInterceptor" /> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        public ChannelHandlerInterceptor(ILightContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
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
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
                                                                                      ServerCallContext context,
                                                                                      UnaryServerMethod<TRequest, TResponse> continuation)
        {
            HandleChannel(context);
            return await continuation(request, context);
        }

        /// <summary>
        /// Handles the channel.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        private void HandleChannel(ServerCallContext context)
        {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            var channelCode = context.GetHeaderValueFromRequest(ChannelTag);
            if (string.IsNullOrEmpty(channelCode)) { channelCode = string.Empty; }
            try
            {
                _contextFactory.GetContext().SetChannel(channelCode);
            }
            catch
            {
                // ignored. Left intentionally blank.
            }
        }
    }
}
