using Liquid.Core.Interfaces;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Http.Tests.Mocks
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
        public static ILiquidContext GetMock()
        {
            var mock = Substitute.For<ILiquidContext>();
            return mock;
        }
    }
}
