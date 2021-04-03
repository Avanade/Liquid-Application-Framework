using Liquid.Core.Telemetry;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Grpc.Factories
{
    /// <summary>
    /// Gets the current telemetry from the current web scope.
    /// </summary>
    /// <seealso cref="ILightTelemetryFactory" />
    [ExcludeFromCodeCoverage]
    internal class GrpcLightTelemetryFactory : ILightTelemetryFactory
    {
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrpcLightTelemetryFactory"/> class.
        /// </summary>
        /// <param name="contextAccessor">The context accessor.</param>
        public GrpcLightTelemetryFactory(IHttpContextAccessor contextAccessor)
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