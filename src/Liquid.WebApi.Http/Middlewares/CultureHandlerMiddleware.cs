using System;
using System.Threading.Tasks;
using Liquid.Core.Configuration;
using Liquid.Core.Context;
using Liquid.Core.Localization;
using Liquid.WebApi.Http.Extensions;
using Microsoft.AspNetCore.Http;

namespace Liquid.WebApi.Http.Middlewares
{
    /// <summary>
    /// Handles the culture code information from request. Checks the culture code either from header or querystring.
    /// </summary>
    public sealed class CultureHandlerMiddleware
    {
        private const string CultureTag = "culture";
        private readonly ILightContextFactory _contextFactory;
        private readonly ILightConfiguration<CultureSettings> _cultureSettings;
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureHandlerMiddleware" /> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="contextFactory">The service provider.</param>
        /// <param name="cultureSettings">The culture settings.</param>
        public CultureHandlerMiddleware(RequestDelegate next, ILightContextFactory contextFactory, ILightConfiguration<CultureSettings> cultureSettings)
        {
            _next = next;
            _contextFactory = contextFactory;
            _cultureSettings = cultureSettings;
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="ArgumentNullException">context</exception>
        public async Task InvokeAsync(HttpContext context)
        {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            var cultureCode = context.GetHeaderValueFromRequest(CultureTag);
            if (string.IsNullOrEmpty(cultureCode)) { cultureCode = context.Request.GetValueFromQuerystring(CultureTag); }
            if (string.IsNullOrEmpty(cultureCode) && !string.IsNullOrEmpty(_cultureSettings.Settings.DefaultCulture)) 
            {
                cultureCode = _cultureSettings.Settings.DefaultCulture;
            }

            if (!string.IsNullOrEmpty(cultureCode))
            {
                try
                {
                    var lightContext = _contextFactory.GetContext();
                    lightContext.SetCulture(cultureCode);
                }
                catch
                {
                    // ignored. Left intentionally blank.
                }
            }
            await _next(context);
        }
    }
}