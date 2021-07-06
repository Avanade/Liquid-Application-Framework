using Liquid.WebApi.Http.Middlewares;
using Liquid.WebApi.Http.Tests.Mocks;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Liquid.WebApi.Http.Tests.TestCases.Middlewares
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ContextDataHandlerMiddlewareTests
    {
        LiquidContextMiddleware _sut;

        /// <summary>
        /// Establishes the context.
        /// </summary>
        //[SetUp]
        //public void EstablishContext()
        //{
        //    _sut = new LiquidContextMiddleware(RequestDelegateMock.GetMock(), ILightContextFactoryMock.GetMock());
        //}

        //[Test]
        //public async Task Verify_InvokeAsync()
        //{
        //    var mock = new DefaultHttpContext();
        //    mock.Request.Headers["contextid"] = "{B079DEE1-CB21-47A1-9006-982E20C562E0}";

        //    await _sut.InvokeAsync(mock);
        //    Assert.AreEqual(Guid.Parse("{B079DEE1-CB21-47A1-9006-982E20C562E0}"), ILightContextFactoryMock.Context.ContextId);
        //}
    }
}
