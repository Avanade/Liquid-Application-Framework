using Liquid.Domain.Extensions.Crud.Commands.AddGenericEntity;
using Liquid.Domain.Extensions.Crud.Commands.RemoveGenericEntity;
using Liquid.Domain.Extensions.Crud.Commands.UpdateGenericEntity;
using Liquid.Domain.Extensions.Crud.Queries.FindByIdGenericEntity;
using Liquid.Domain.Extensions.Crud.Queries.GetAllGenericEntity;
using Liquid.WebApi.Http.Extensions.Crud.Controllers;
using Liquid.WebApi.Http.Extensions.Crud.Tests.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.WebApi.Http.Extensions.Crud.Tests.TestCases
{
    public class GenericCrudControllerTests
    {
        [Fact]
        public async Task Test_AddAsync_Success()
        {
            //Arrrange
            var product = new Product();
            var mediator = new Mock<IMediator>();
            var controller = new GenericCrudController<Product, int>(mediator.Object);

            //Act
            var result = await controller.AddAsync(product);

            //Assert
            mediator.Verify(o => o.Send(It.Is<AddGenericEntityCommand<Product, int>>(o => o.Data == product), It.IsAny<CancellationToken>()), Times.Once());
            Assert.IsAssignableFrom<CreatedAtRouteResult>(result);
        }

        [Fact]
        public async Task Test_GetAll_Success()
        {
            //Arrange
            IEnumerable<Product> products = new List<Product>() { new Product() };
            var mediator = new Mock<IMediator>();
            mediator.Setup(o => o.Send(It.IsAny<GetAllGenericEntityQuery<Product, int>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new GetAllGenericEntityQueryResponse<Product>(products)));
            var controller = new GenericCrudController<Product, int>(mediator.Object);

            //Act
            var result = await controller.GetAllAsync();

            //Assert
            mediator.Verify(o => o.Send(It.IsAny<GetAllGenericEntityQuery<Product, int>>(), It.IsAny<CancellationToken>()), Times.Once());
            Assert.IsAssignableFrom<OkObjectResult>(result);
            var response = (OkObjectResult)result;
            Assert.Equal(products, response.Value);
        }

        [Fact]
        public async Task Test_GetAll_NotFound()
        {
            //Arrange
            var mediator = new Mock<IMediator>();
            mediator.Setup(o => o.Send(It.IsAny<GetAllGenericEntityQuery<Product, int>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new GetAllGenericEntityQueryResponse<Product>(null)));
            var controller = new GenericCrudController<Product, int>(mediator.Object);

            //Act
            var result = await controller.GetAllAsync();

            //Assert
            mediator.Verify(o => o.Send(It.IsAny<GetAllGenericEntityQuery<Product, int>>(), It.IsAny<CancellationToken>()), Times.Once());
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Fact]
        public async Task Test_GetByIdAsync_Success()
        {
            //Arrrange
            var product = new Product();
            var mediator = new Mock<IMediator>();
            mediator.Setup(o => o.Send(It.IsAny<FindByIdGenericEntityQuery<Product, int>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new FindByIdGenericEntityQueryResponse<Product>(product)));
            var controller = new GenericCrudController<Product, int>(mediator.Object);

            //Act
            var result = await controller.GetByIdAsync(product.Id);

            //Assert
            mediator.Verify(o => o.Send(It.Is<FindByIdGenericEntityQuery<Product, int>>(o => o.Id == product.Id), It.IsAny<CancellationToken>()), Times.Once());
            Assert.IsAssignableFrom<OkObjectResult>(result);
            var response = (OkObjectResult)result;
            Assert.Equal(product, response.Value);
        }

        [Fact]
        public async Task Test_GetByIdAsync_NotFound()
        {
            //Arrrange
            var product = new Product();
            var mediator = new Mock<IMediator>();
            mediator.Setup(o => o.Send(It.IsAny<FindByIdGenericEntityQuery<Product, int>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new FindByIdGenericEntityQueryResponse<Product>(null)));
            var controller = new GenericCrudController<Product, int>(mediator.Object);

            //Act
            var result = await controller.GetByIdAsync(product.Id);

            //Assert
            mediator.Verify(o => o.Send(It.Is<FindByIdGenericEntityQuery<Product, int>>(o => o.Id == product.Id), It.IsAny<CancellationToken>()), Times.Once());
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Fact]
        public async Task Test_RemoveAsync_Success()
        {
            //Arrrange
            var product = new Product();
            var mediator = new Mock<IMediator>();
            mediator.Setup(o => o.Send(It.IsAny<RemoveGenericEntityCommand<Product, int>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new RemoveGenericEntityCommandResponse<Product>(product)));
            var controller = new GenericCrudController<Product, int>(mediator.Object);

            //Act
            var result = await controller.RemoveAsync(product.Id);

            //Assert
            mediator.Verify(o => o.Send(It.Is<RemoveGenericEntityCommand<Product, int>>(o => o.Id == product.Id), It.IsAny<CancellationToken>()), Times.Once());
            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public async Task Test_RemoveAsync_NotFound()
        {
            //Arrrange
            var product = new Product();
            var mediator = new Mock<IMediator>();
            mediator.Setup(o => o.Send(It.IsAny<RemoveGenericEntityCommand<Product, int>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new RemoveGenericEntityCommandResponse<Product>(null)));
            var controller = new GenericCrudController<Product, int>(mediator.Object);

            //Act
            var result = await controller.RemoveAsync(product.Id);

            //Assert
            mediator.Verify(o => o.Send(It.Is<RemoveGenericEntityCommand<Product, int>>(o => o.Id == product.Id), It.IsAny<CancellationToken>()), Times.Once());
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Fact]
        public async Task Test_UpdateAsync_Success()
        {
            //Arrrange
            var product = new Product();
            var mediator = new Mock<IMediator>();
            mediator.Setup(o => o.Send(It.IsAny<UpdateGenericEntityCommand<Product, int>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new UpdateGenericEntityCommandResponse<Product>(product)));
            var controller = new GenericCrudController<Product, int>(mediator.Object);

            //Act
            var result = await controller.UpdateAsync(product);

            //Assert
            mediator.Verify(o => o.Send(It.Is<UpdateGenericEntityCommand<Product, int>>(o => o.Data == product), It.IsAny<CancellationToken>()), Times.Once());
            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public async Task Test_UpdateAsync_NotFound()
        {
            //Arrrange
            var product = new Product();
            var mediator = new Mock<IMediator>();
            mediator.Setup(o => o.Send(It.IsAny<UpdateGenericEntityCommand<Product, int>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new UpdateGenericEntityCommandResponse<Product>(null)));
            var controller = new GenericCrudController<Product, int>(mediator.Object);

            //Act
            var result = await controller.UpdateAsync(product);

            //Assert
            mediator.Verify(o => o.Send(It.Is<UpdateGenericEntityCommand<Product, int>>(o => o.Data == product), It.IsAny<CancellationToken>()), Times.Once());
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }
    }
}
