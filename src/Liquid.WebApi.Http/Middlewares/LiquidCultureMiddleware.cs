using Liquid.Core.Interfaces;
using Liquid.Core.Settings;
using Liquid.WebApi.Http.Extensions;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;

namespace Liquid.WebApi.Http.Middlewares
{
    /// <summary>
    /// Configures the culture in the current thread.
    /// Includes its behavior in netcore pipelines before request execution.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class LiquidCultureMiddleware
    {
        private const string _culture = "culture";
        private readonly ILiquidConfiguration<CultureSettings> _options;
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiquidCultureMiddleware" /> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="options"></param>
        public LiquidCultureMiddleware(RequestDelegate next, ILiquidConfiguration<CultureSettings> options)
        {
            _next = next;
            _options = options;
        }

        /// <summary>
        /// Configures the culture in the current thread as set on request or in appsettings, prioritizing request informations.
        /// Includes its behavior in netcore pipelines before request execution.
        /// </summary>
        /// <param name="context">HTTP-specific information about an individual HTTP request.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            var cultureCode = context.GetValueFromHeader(_culture).ToString();

            if (string.IsNullOrEmpty(cultureCode))
            {
                cultureCode = context.GetValueFromQuery(_culture).ToString();
            }

            if (string.IsNullOrEmpty(cultureCode) && !string.IsNullOrEmpty(_options.Settings.DefaultCulture))
            {
                cultureCode = _options.Settings.DefaultCulture;
            }

            if (!string.IsNullOrEmpty(cultureCode))
            {
                CultureInfo.CurrentCulture = new CultureInfo(cultureCode);
                CultureInfo.CurrentUICulture = new CultureInfo(cultureCode);
            }

            await _next(context);
        }
    }
}