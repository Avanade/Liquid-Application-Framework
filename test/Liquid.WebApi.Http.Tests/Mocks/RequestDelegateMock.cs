using Microsoft.AspNetCore.Http;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Http.Tests.Mocks
{
    /// <summary>
    /// RequestDelegate Mock, returns the mock interface for tests purpose.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RequestDelegateMock
    {
        /// <summary>
        /// Gets the RequestDelegate mock class.
        /// </summary>
        /// <returns></returns>
        public static RequestDelegate GetMock()
        {
            var mock = Substitute.For<RequestDelegate>();
            return mock;
        }
    }
}
