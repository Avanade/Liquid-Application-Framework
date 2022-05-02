using Liquid.Messaging.Tests.Mock;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Liquid.Messaging.Extensions.DependencyInjection;
using Liquid.Messaging.Interfaces;
using MediatR;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using Liquid.Core.Interfaces;
using Liquid.Core.Settings;
using Liquid.Core.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Liquid.Messaging.Tests
{
    public class IServiceCollectionExtensionTest
    {
        private IServiceCollection _services;
        private IServiceProvider _serviceProvider;

        public IServiceCollectionExtensionTest()
        {
            _services = new ServiceCollection();
        }


        [Fact]
        public void AddLiquidWorkerService_WhenAdded_ServiceProvideCanResolveHostedService()
        {
            _services.AddSingleton(Substitute.For<ILiquidConsumer<EntityMock>>());
            _services.AddLiquidWorkerService<WorkerMock, EntityMock>();
            _serviceProvider = _services.BuildServiceProvider();

            Assert.NotNull(_serviceProvider.GetService<ILiquidWorker<EntityMock>>());
            Assert.NotNull(_serviceProvider.GetService<ILiquidConsumer<EntityMock>>());
            Assert.NotNull(_serviceProvider.GetService<IHostedService>());
        }

        [Fact]
        public void AddLiquidDomain_WhenAdded_ServiceProviderCanResolveMediatorService()
        {
            _services.AddLiquidDomain(typeof(CommandRequestMock).Assembly);
            _serviceProvider = _services.BuildServiceProvider();

            Assert.NotNull(_serviceProvider.GetService<IMediator>());
        }

        [Fact]
        public void AddLiquidPipeline_WhenAdded_ServiceProviderCanResolveMediatorService()
        {
            _services.AddLiquidConfiguration();
            _services.AddSingleton(typeof(IOptions<>));
            _services.AddSingleton<ILiquidWorker<EntityMock>, WorkerMock>();
            _services.AddSingleton(Substitute.For<ILiquidConsumer<EntityMock>>());
            _services.AddLiquidPipeline<EntityMock>();
            _serviceProvider = _services.BuildServiceProvider();

            Assert.NotNull(_serviceProvider.GetService<ILiquidWorker<EntityMock>>());
        }


    }
}
