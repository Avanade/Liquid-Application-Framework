using Liquid.WebApi.Http.Middlewares;
using Liquid.WebApi.Http.Tests.Mocks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Liquid.WebApi.Http.Tests.TestCases.Middlewares
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ChannelHandlerMiddlewareTests
    {
        ChannelHandlerMiddleware _sut;

        /// <summary>
        /// Establishes the context.
        /// </summary>
        [SetUp]
        public void EstablishContext()
        {
            _sut = new ChannelHandlerMiddleware(RequestDelegateMock.GetMock(), ILightContextFactoryMock.GetMock());
        }

        [Test]
        public async Task Verify_InvokeAsync()
        {
            var mock = new DefaultHttpContext();

            //Assert channel null
            await _sut.InvokeAsync(mock);
            Assert.AreEqual(null, ILightContextFactoryMock.Context.ContextChannel);

            //Assert channel from queryString
            mock.Request.Query = new QueryCollection(new Dictionary<string, StringValues> { { "channel", "web" } });
            await _sut.InvokeAsync(mock);
            Assert.AreEqual("web", ILightContextFactoryMock.Context.ContextChannel);

            //Assert channel from Header
            mock.Request.Headers["channel"] = "mobile";
            await _sut.InvokeAsync(mock);
            Assert.AreEqual("mobile", ILightContextFactoryMock.Context.ContextChannel);
        }
    }
}
