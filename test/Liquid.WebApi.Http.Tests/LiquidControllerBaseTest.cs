using Liquid.WebApi.Http.UnitTests.Mocks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.WebApi.Http.UnitTests
{
    public class LiquidControllerBaseTest
    {
        private TestController _sut;
        private IMediator _mediator;

        public LiquidControllerBaseTest()
        {
            _mediator = Substitute.For<IMediator>();
            _sut = new TestController(_mediator);
        }


        [Fact]
        public async Task ExecuteAsync_WhenIActionResultOverload_Return200()
        {
            var response = await _sut.GetCase1();

            var result = (ObjectResult)response;

            await _mediator.Received(1).Send(Arg.Any<TestCaseRequest>());

            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task ExecuteAsync_WhenGenericResultOverload_Return200()
        {
            var response = await _sut.GetCase2();

            var result = (ObjectResult)response;

            await _mediator.Received(1).Send(Arg.Any<TestCaseRequest>());

            Assert.Equal(200, result.StatusCode);
        }
    }
}
