using Liquid.Domain.Extensions.Crud.Commands.AddGenericEntity;
using Liquid.Domain.Extensions.Crud.Notifications.GenericEntityAdded;
using Liquid.Domain.Extensions.Crud.Tests.Entities;
using Liquid.Repository;
using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Domain.Extensions.Crud.Tests.TestCases
{
    public class AddGenericEntityCommandHandlerTests
    {
        [Fact]
        public async Task Test_WhenHandle_Success()
        {
            var repository = new Mock<ILiquidRepository<Product, int>>();
            var mediator = new Mock<IMediator>();
            var handler = new AddGenericEntityCommandHandler<Product, int>(repository.Object, mediator.Object);
            var product = new Product();
            var request = new AddGenericEntityCommand<Product, int>(product);

            var response = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(response);
            Assert.IsAssignableFrom<AddGenericEntityCommandResponse<Product>>(response);
            Assert.Equal(product, response.Data);
            repository.Verify(o => o.AddAsync(product), Times.Once());
            mediator.Verify(o => o.Publish(It.Is<GenericEntityAddedNotification<Product, int>>(o => o.Data == product), It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
