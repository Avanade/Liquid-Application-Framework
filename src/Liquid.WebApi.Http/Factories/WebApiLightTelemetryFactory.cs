using Liquid.Core.Telemetry;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Http.Factories
{
    /// <summary>
    /// Gets the current telemetry from the current web scope.
    /// </summary>
    /// <seealso cref="ILightTelemetryFactory" />
    [ExcludeFromCodeCoverage]
    internal class WebApiLightTelemetryFactory : ILightTelemetryFactory
    {
        private readonly IHttpContextAccessor _contextAccessor;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiLightTelemetryFactory"/> class.
        /// </summary>
        /// <param name="contextAccessor">The context accessor.</param>
        public WebApiLightTelemetryFactory(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Gets the telemetry.
        /// </summary>
        /// <returns></returns>
        public ILightTelemetry GetTelemetry()
        {
            return _contextAccessor.HttpContext.RequestServices.GetService<ILightTelemetry>();
        }
    }
}