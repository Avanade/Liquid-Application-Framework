using Liquid.Domain.Extensions.Crud.Commands.AddGenericEntity;
using Liquid.Domain.Extensions.Crud.Commands.RemoveGenericEntity;
using Liquid.Domain.Extensions.Crud.Commands.UpdateGenericEntity;
using Liquid.Domain.Extensions.Crud.Queries.FindByIdGenericEntity;
using Liquid.Domain.Extensions.Crud.Queries.GetAllGenericEntity;
using Liquid.Domain.Extensions.Crud.Tests.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace Liquid.Domain.Extensions.Crud.Tests
{
    public class DependencyInjectionExtensionsTests
    {
        [Theory]
        [InlineData(typeof(IRequestHandler<AddGenericEntityCommand<Product, int>, AddGenericEntityCommandResponse<Product>>), typeof(AddGenericEntityCommandHandler<Product, int>))]
        [InlineData(typeof(IRequestHandler<FindByIdGenericEntityQuery<Product, int>, FindByIdGenericEntityQueryResponse<Product>>), typeof(FindByIdGenericEntityQueryHandler<Product, int>))]
        [InlineData(typeof(IRequestHandler<GetAllGenericEntityQuery<Product, int>, GetAllGenericEntityQueryResponse<Product>>), typeof(GetAllGenericEntityQueryHandler<Product, int>))]
        [InlineData(typeof(IRequestHandler<RemoveGenericEntityCommand<Product, int>, RemoveGenericEntityCommandResponse<Product>>), typeof(RemoveGenericEntityCommandHandler<Product, int>))]
        [InlineData(typeof(IRequestHandler<UpdateGenericEntityCommand<Product, int>, UpdateGenericEntityCommandResponse<Product>>), typeof(UpdateGenericEntityCommandHandler<Product, int>))]
        public void Test_WhenRegisterCrud_Sucess(Type serviceType, Type implementationType)
        {
            //Arrange
            var services = new ServiceCollection();

            //Act
            services.RegisterCrud<Product, int>();

            //Assert
            var serviceDescriptor = services.Where(s => s.ServiceType == serviceType);
            Assert.NotNull(serviceDescriptor);
            Assert.NotEmpty(serviceDescriptor);

            Assert.Equal(implementationType, serviceDescriptor.FirstOrDefault().ImplementationType);
        }
    }
}
