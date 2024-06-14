using Liquid.Core.Decorators;
using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Core.Interfaces;
using Liquid.Core.Settings;
using Liquid.Core.Tests.Mocks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using Xunit;

namespace Liquid.Core.Tests.Messaging
{
    public class IServiceCollectionExtensionTest
    {
        private IServiceCollection _services;
        private IServiceProvider _serviceProvider;
        private IConfiguration _configProvider = Substitute.For<IConfiguration>();
        private IConfigurationSection _configurationSection = Substitute.For<IConfigurationSection>();

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
        public void AddLiquidPipeline_WhenAdded_ServiceProviderCanResolveLiquidWorkerService()
        {
            ConfigureServices();

            _services.AddSingleton<ILiquidWorker<EntityMock>, WorkerMock>();
            _services.AddSingleton(Substitute.For<ILiquidConsumer<EntityMock>>());

            _services.AddLiquidPipeline<EntityMock>();
            _serviceProvider = _services.BuildServiceProvider();

            Assert.NotNull(_serviceProvider.GetService<ILiquidWorker<EntityMock>>());
        }
        [Fact]
        public void AddLiquidMessageConsumer_WhenAdded_ServiceProviderCanResolveLiquidMessagingConsumerServices()
        {
            ConfigureServices();

            _services.AddSingleton(Substitute.For<ILiquidConsumer<EntityMock>>());

            _services.AddLiquidMessageConsumer<WorkerMock, EntityMock>(typeof(CommandRequestMock).Assembly);

            _serviceProvider = _services.BuildServiceProvider();

            Assert.NotNull(_serviceProvider.GetService<ILiquidWorker<EntityMock>>());
            Assert.NotNull(_serviceProvider.GetService<IHostedService>());
            Assert.NotNull(_serviceProvider.GetService<IMediator>());
        }

        private void ConfigureServices()
        {
            _configProvider.GetSection(Arg.Any<string>()).Returns(_configurationSection);
            _services.AddSingleton(_configProvider);
            _services.AddSingleton(Substitute.For<IOptions<ScopedContextSettings>>());
            _services.AddSingleton(Substitute.For<IOptions<ScopedLoggingSettings>>());
            _services.AddSingleton(Substitute.For<IOptions<CultureSettings>>());
            _services.AddSingleton(Substitute.For<ILiquidConfiguration<ScopedContextSettings>>());
            _services.AddSingleton(Substitute.For<ILiquidConfiguration<ScopedLoggingSettings>>());
            _services.AddSingleton(Substitute.For<ILiquidConfiguration<CultureSettings>>());
            _services.AddSingleton(Substitute.For<ILogger<LiquidScopedLoggingDecorator<EntityMock>>>());
        }
    }
}
