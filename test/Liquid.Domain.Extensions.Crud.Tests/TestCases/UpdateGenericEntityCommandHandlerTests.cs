using Liquid.Domain.Extensions.Crud.Commands.UpdateGenericEntity;
using Liquid.Domain.Extensions.Crud.Notifications.GenericEntityUpdated;
using Liquid.Domain.Extensions.Crud.Tests.Entities;
using Liquid.Repository;
using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Domain.Extensions.Crud.Tests.TestCases
{
    public class UpdateGenericEntityCommandHandlerTests
    {
        [Fact]
        public async Task Test_WhenHandle_Success()
        {
            const int PRODUCT_ID = 0;

            var product = new Product() { Id = PRODUCT_ID };
            var repository = new Mock<ILiquidRepository<Product, int>>();
            repository.Setup(o => o.FindByIdAsync(PRODUCT_ID)).Returns(Task.FromResult(product));
            var mediator = new Mock<IMediator>();
            var handler = new UpdateGenericEntityCommandHandler<Product, int>(repository.Object, mediator.Object);
            var request = new UpdateGenericEntityCommand<Product, int>(product);

            var response = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(response);
            Assert.IsAssignableFrom<UpdateGenericEntityCommandResponse<Product>>(response);
            Assert.Equal(product, response.Data);
            repository.Verify(o => o.FindByIdAsync(PRODUCT_ID), Times.Once());
            repository.Verify(o => o.UpdateAsync(product), Times.Once());
            mediator.Verify(o => o.Publish(It.Is<GenericEntityUpdatedNotification<Product, int>>(o => o.Data == product), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Test_WhenHandle_NotFound()
        {
            const int PRODUCT_ID = 0;

            var product = new Product() { Id = PRODUCT_ID };
            var repository = new Mock<ILiquidRepository<Product, int>>();
            repository.Setup(o => o.FindByIdAsync(PRODUCT_ID)).Returns(Task.FromResult<Product>(null));
            var mediator = new Mock<IMediator>();
            var handler = new UpdateGenericEntityCommandHandler<Product, int>(repository.Object, mediator.Object);
            var request = new UpdateGenericEntityCommand<Product, int>(product);

            var response = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(response);
            Assert.IsAssignableFrom<UpdateGenericEntityCommandResponse<Product>>(response);
            repository.Verify(o => o.FindByIdAsync(PRODUCT_ID), Times.Once());
            repository.Verify(o => o.UpdateAsync(product), Times.Never());
            mediator.Verify(o => o.Publish(It.Is<GenericEntityUpdatedNotification<Product, int>>(o => o.Data == product), It.IsAny<CancellationToken>()), Times.Never());
        }
    }
}
