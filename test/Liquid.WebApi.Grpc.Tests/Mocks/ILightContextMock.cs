using Liquid.Core.Context;
using NSubstitute;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Grpc.Tests.Mocks
{
    /// <summary>
    /// ILightContext Mock, returns the mock interface for tests purpose.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ILightContextMock
    {
        /// <summary>
        /// Gets the ILightContext mock.
        /// </summary>
        /// <returns></returns>
        public static ILightContext GetMock()
        {
            var mock = Substitute.For<ILightContext>();
            mock.GetNotifications().ReturnsForAnyArgs(new Dictionary<string, object>() { { "teste", "teste" } });
            return mock;
        }
    }
}
