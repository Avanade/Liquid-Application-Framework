using Liquid.Core.Context;
using Liquid.Core.Localization;
using Liquid.Core.Telemetry;
using Liquid.WebApi.Http.Controllers;
using Liquid.WebApi.Http.Tests.Mocks;
using MediatR;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Liquid.WebApi.Http.Tests.TestCases.Controllers
{
    /// <summary>
    /// Base controller test case
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class BaseControllerTests
    {
        /// <summary>
        /// The sut
        /// </summary>
        private ControllerTest _sut;

        /// <summary>
        /// Establishes the context.
        /// </summary>
        [SetUp]
        public void EstablishContext()
        {
            _sut = new ControllerTest(ILoggerFactoryMock.GetMock(),
                                      IMediatorMock.GetMock(),
                                      ILightContextMock.GetMock(),
                                      ILightTelemetryMock.GetMock(),
                                      ILocalizationMock.GetMock());
        }

        /// <summary>
        /// Verifies the execute asynchronous.
        /// </summary>
        [Test]
        public async Task Verify_ExecuteAsync()
        {
            await _sut.TestExecuteAsync();
        }

    }

    /// <summary>
    /// Dummy Controller for test purpose.
    /// </summary>
    /// <seealso cref="Liquid.WebApi.Http.Controllers.BaseController" />
    internal class ControllerTest : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerTest"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="mediator">The mediator service.</param>
        /// <param name="context">The current context.</param>
        /// <param name="telemetry">The current telemetry.</param>
        /// <param name="localization">The localization service.</param>
        public ControllerTest(ILoggerFactory loggerFactory, 
                              IMediator mediator, 
                              ILightContext context, 
                              ILightTelemetry telemetry, 
                              ILocalization localization) : base(loggerFactory, mediator, context, telemetry, localization)
        {
        }

        /// <summary>
        /// Tests the execute asynchronous.
        /// </summary>
        public async Task TestExecuteAsync()
        {
            await ExecuteAsync(new Request());
            await ExecuteAsync(new Request(), System.Net.HttpStatusCode.OK);
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
