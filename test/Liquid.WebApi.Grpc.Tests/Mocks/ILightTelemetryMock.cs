using Liquid.Core.Telemetry;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Grpc.Tests.Mocks
{
    /// <summary>
    /// ILightTelemetry Mock, returns the mock interface for tests purpose.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ILightTelemetryMock
    {
        /// <summary>
        /// Gets the ILightTelemetry mock.
        /// </summary>
        /// <returns></returns>
        public static ILightTelemetry GetMock()
        {
            var mock = Substitute.For<ILightTelemetry>();
            return mock;
        }
    }
}
