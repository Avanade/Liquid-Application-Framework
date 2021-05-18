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
    public class CultureHandlerMiddlewareTests
    {
        CultureHandlerMiddleware _sut;

        /// <summary>
        /// Establishes the context.
        /// </summary>
        [SetUp]
        public void EstablishContext()
        {
            _sut = new CultureHandlerMiddleware(RequestDelegateMock.GetMock(), 
                                                ILightContextFactoryMock.GetMock(), 
                                                ILightConfigurationCultureSettingsMock.GetMock());
        }

        [Test]
        public async Task Verify_InvokeAsync()
        {
            var mock = new DefaultHttpContext();

            //Assert default culture.
            await _sut.InvokeAsync(mock);
            Assert.AreEqual("en-US", ILightContextFactoryMock.Context.ContextCulture);

            //Assert culture from queryString
            mock.Request.Query = new QueryCollection(new Dictionary<string, StringValues> { { "culture", "pt-BR" } });
            await _sut.InvokeAsync(mock);
            Assert.AreEqual("pt-BR", ILightContextFactoryMock.Context.ContextCulture);

            //Assert culture from Header
            mock.Request.Headers["culture"] = "fr-FR";
            await _sut.InvokeAsync(mock);
            Assert.AreEqual("fr-FR", ILightContextFactoryMock.Context.ContextCulture);
        }
    }
}
