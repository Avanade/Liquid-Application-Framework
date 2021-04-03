using Grpc.Core;
using Liquid.WebApi.Grpc.Tests.Mocks;
using MediatR;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Liquid.WebApi.Grpc.Tests.TestCases.Service
{
    /// <summary>
    /// Light Grpc Service Tests class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class LightGrpcServiceTests
    {
        /// <summary>
        /// Verifies the execute asynchronous.
        /// </summary>
        [Test]
        public async Task Verify_ExecuteAsync()
        {
            var sut = new LightGrpcService(IMediatorMock.GetMock(),
                                           ILightContextMock.GetMock(),
                                           ILightTelemetryMock.GetMock(),
                                           ILocalizationMock.GetMock());
            await sut.ExecuteAsync(new Request(), ServerCallContextMock.GetMock());
        }

        /// <summary>
        /// Verifies the execute asynchronous with Validation Errors.
        /// </summary>
        [Test]
        public void Verify_ExecuteAsync_WithValidatiorErrors()
        {
            var sut = new LightGrpcService(IMediatorMock.GetMockWithValidationErrors(),
                                           ILightContextMock.GetMock(),
                                           ILightTelemetryMock.GetMock(),
                                           ILocalizationMock.GetMock());

            Assert.ThrowsAsync<RpcException>(async () => await sut.ExecuteAsync(new Request(), ServerCallContextMock.GetMock()));             
        }

        /// <summary>
        /// Verifies the execute asynchronous with custom Error.
        /// </summary>
        [Test]
        public void Verify_ExecuteAsync_WithCustomErrors()
        {
            var sut = new LightGrpcService(IMediatorMock.GetMockWithCustomErrors(),
                                           ILightContextMock.GetMock(),
                                           ILightTelemetryMock.GetMock(),
                                           ILocalizationMock.GetMock());

            Assert.ThrowsAsync<RpcException>(async () => await sut.ExecuteAsync(new Request(), ServerCallContextMock.GetMock()));
        }
    }

    /// <summary>
    /// Dummy Request for test purpose.
    /// </summary>
    /// <seealso cref="MediatR.IRequest{System.Boolean}" />
    internal class Request : IRequest<bool>
    {
    }
}
