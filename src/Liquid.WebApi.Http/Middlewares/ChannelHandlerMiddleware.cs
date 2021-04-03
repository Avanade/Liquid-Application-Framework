using System;
using System.Threading.Tasks;
using Liquid.Core.Context;
using Liquid.WebApi.Http.Extensions;
using Microsoft.AspNetCore.Http;

namespace Liquid.WebApi.Http.Middlewares
{
    /// <summary>
    /// Channel Handler Middleware class.
    /// </summary>
    public class ChannelHandlerMiddleware
    {
        private const string ChannelTag = "channel";
        private readonly RequestDelegate _next;
        private readonly ILightContextFactory _contextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelHandlerMiddleware" /> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="contextFactory">The context factory.</param>
        public ChannelHandlerMiddleware(RequestDelegate next, ILightContextFactory contextFactory)
        {
            _next = next;
            _contextFactory = contextFactory;
        }

        /// <summary>
        /// Invokes the asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="ArgumentNullException">context</exception>
        public async Task InvokeAsync(HttpContext context)
        {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            var channelCode = context.GetHeaderValueFromRequest(ChannelTag);
            if (string.IsNullOrEmpty(channelCode)) { channelCode = context.Request.GetValueFromQuerystring(ChannelTag); }
            if (string.IsNullOrEmpty(channelCode)) { channelCode = string.Empty; }
            SetCurrentChannel(channelCode);
            await _next(context);
        }

        /// <summary>
        /// Sets the current channel.
        /// </summary>
        /// <param name="channelCode">The channel code.</param>
        private void SetCurrentChannel(string channelCode)
        {
            if (string.IsNullOrEmpty(channelCode)) return;

            try
            {
                var lightContext = _contextFactory.GetContext();
                lightContext.SetChannel(channelCode);
            }
            catch
            {
                // ignored. Left intentionally blank.
            }
        }
    }
}
