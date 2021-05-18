using Grpc.Core;
using NSubstitute;

namespace Liquid.WebApi.Grpc.Tests.Mocks
{
    /// <summary>
    /// ServerCallContext Mock, returns the mock interface for tests purpose.
    /// </summary>
    public class ServerCallContextMock
    {
        /// <summary>
        /// Gets the ServerCallContext mock.
        /// </summary>
        /// <returns></returns>
        public static ServerCallContext GetMock()
        {
            var mock = Substitute.For<ServerCallContext>();
            mock.ResponseTrailers.ReturnsForAnyArgs(new Metadata());
            return mock;
        }
    }
}
