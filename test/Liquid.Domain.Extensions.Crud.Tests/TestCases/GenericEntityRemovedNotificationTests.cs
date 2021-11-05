using Liquid.Domain.Extensions.Crud.Notifications.GenericEntityRemoved;
using Liquid.Domain.Extensions.Crud.Tests.Entities;
using Xunit;

namespace Liquid.Domain.Extensions.Crud.Tests.TestCases
{
    public class GenericEntityRemovedNotificationTests
    {
        [Fact]
        public void Test_WhenConstruct_Success()
        {
            //Arrange
            var product = new Product();

            //Act
            var notification = new GenericEntityRemovedNotification<Product, int>(product);

            //Assert
            Assert.Equal(product.Id, notification.Id);
            Assert.Equal(product, notification.Data);
            Assert.Equal("Product", notification.EntityName);
        }
    }
}
