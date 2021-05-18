using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Http.Tests.Mocks
{
    /// <summary>
    /// ILoggerFactory Mock, returns the mock interface for tests purpose.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ILoggerFactoryMock
    {
        /// <summary>
        /// Gets the ILoggerFactory mock.
        /// </summary>
        /// <returns></returns>
        public static ILoggerFactory GetMock()
        {
            var mock = Substitute.For<ILoggerFactory>();
            return mock;
        }
    }
}
