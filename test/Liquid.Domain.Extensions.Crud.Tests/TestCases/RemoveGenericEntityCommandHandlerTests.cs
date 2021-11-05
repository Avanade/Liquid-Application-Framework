using Liquid.Domain.Extensions.Crud.Commands.RemoveGenericEntity;
using Liquid.Domain.Extensions.Crud.Notifications.GenericEntityRemoved;
using Liquid.Domain.Extensions.Crud.Tests.Entities;
using Liquid.Repository;
using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Domain.Extensions.Crud.Tests.TestCases
{
    public class RemoveGenericEntityCommandHandlerTests
    {
        [Fact]
        public async Task Test_WhenHandle_Success()
        {
            const int PRODUCT_ID = 0;

            var product = new Product() { Id = PRODUCT_ID };
            var repository = new Mock<ILiquidRepository<Product, int>>();
            repository.Setup(o => o.FindByIdAsync(PRODUCT_ID)).Returns(Task.FromResult(product));
            var mediator = new Mock<IMediator>();
            var handler = new RemoveGenericEntityCommandHandler<Product, int>(repository.Object, mediator.Object);
            var request = new RemoveGenericEntityCommand<Product, int>(product.Id);

            var response = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(response);
            Assert.IsAssignableFrom<RemoveGenericEntityCommandResponse<Product>>(response);
            repository.Verify(o => o.FindByIdAsync(PRODUCT_ID), Times.Once());
            repository.Verify(o => o.RemoveByIdAsync(PRODUCT_ID), Times.Once());
            mediator.Verify(o => o.Publish(It.Is<GenericEntityRemovedNotification<Product, int>>(o => o.Data == product), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Test_WhenHandle_NotFound()
        {
            const int PRODUCT_ID = 0;

            var product = new Product() { Id = PRODUCT_ID };
            var repository = new Mock<ILiquidRepository<Product, int>>();
            repository.Setup(o => o.FindByIdAsync(PRODUCT_ID)).Returns(Task.FromResult<Product>(null));
            var mediator = new Mock<IMediator>();
            var handler = new RemoveGenericEntityCommandHandler<Product, int>(repository.Object, mediator.Object);
            var request = new RemoveGenericEntityCommand<Product, int>(product.Id);

            var response = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(response);
            Assert.IsAssignableFrom<RemoveGenericEntityCommandResponse<Product>>(response);
            Assert.Null(response.Data);
            repository.Verify(o => o.FindByIdAsync(PRODUCT_ID), Times.Once());
            repository.Verify(o => o.RemoveByIdAsync(PRODUCT_ID), Times.Never());
            mediator.Verify(o => o.Publish(It.Is<GenericEntityRemovedNotification<Product, int>>(o => o.Data == product), It.IsAny<CancellationToken>()), Times.Never());
        }
    }
}
