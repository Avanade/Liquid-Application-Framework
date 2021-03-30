using System;
using Liquid.Core.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Liquid.Core.Telemetry
{
    /// <summary>
    /// Light Telemetry Factory.
    /// </summary>
    /// <seealso cref="Liquid.Core.Telemetry.ILightTelemetryFactory" />
    public class LightTelemetryFactory : ILightTelemetryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightContextFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <exception cref="ArgumentNullException">container</exception>
        public LightTelemetryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        }

        /// <summary>
        /// Gets the telemetry.
        /// </summary>
        /// <returns></returns>
        public ILightTelemetry GetTelemetry() => _serviceProvider.GetRequiredService<ILightTelemetry>();
    }
}