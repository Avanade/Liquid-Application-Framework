using Liquid.Domain.Extensions.Crud.Queries.FindByIdGenericEntity;
using Liquid.Domain.Extensions.Crud.Tests.Entities;
using Liquid.Repository;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Domain.Extensions.Crud.Tests.TestCases
{
    public class FindByIdGenericEntityQueryHandlerTests
    {
        [Fact]
        public async Task Test_WhenHandle_Success()
        {
            const int PRODUCT_ID = 0;
            var product = new Product() { Id = PRODUCT_ID };
            var repository = new Mock<ILiquidRepository<Product, int>>();
            repository.Setup(o => o.FindByIdAsync(PRODUCT_ID)).Returns(Task.FromResult(product));
            var handler = new FindByIdGenericEntityQueryHandler<Product, int>(repository.Object);
            var request = new FindByIdGenericEntityQuery<Product, int>(PRODUCT_ID);

            var response = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(response);
            Assert.IsAssignableFrom<FindByIdGenericEntityQueryResponse<Product>>(response);
            Assert.Equal(product, response.Data);
            repository.Verify(o => o.FindByIdAsync(PRODUCT_ID), Times.Once());
        }
    }
}
