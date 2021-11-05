using Liquid.Domain.Extensions.Crud.Queries.GetAllGenericEntity;
using Liquid.Domain.Extensions.Crud.Tests.Entities;
using Liquid.Repository;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Domain.Extensions.Crud.Tests.TestCases
{
    public class GetAllGenericEntityQueryHandlerTests
    {
        [Fact]
        public async Task Test_WhenHandle_Success()
        {
            var products = new List<Product>();
            var repository = new Mock<ILiquidRepository<Product, int>>();
            repository.Setup(o => o.FindAllAsync()).Returns(Task.FromResult<IEnumerable<Product>>(products));
            var handler = new GetAllGenericEntityQueryHandler<Product, int>(repository.Object);
            var request = new GetAllGenericEntityQuery<Product, int>();

            var response = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(response);
            Assert.IsAssignableFrom<GetAllGenericEntityQueryResponse<Product>>(response);
            Assert.Equal(products, response.Data);
            repository.Verify(o => o.FindAllAsync(), Times.Once());
        }
    }
}
