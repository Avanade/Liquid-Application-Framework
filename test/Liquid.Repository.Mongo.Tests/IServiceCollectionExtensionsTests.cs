using Liguid.Repository.Configuration;
using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Liquid.Repository.Configuration;
using Liquid.Repository.Mongo.Configuration;
using Liquid.Repository.Mongo.Tests.Mock;
using Liquid.Repository.Mongo.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mongo2Go;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Liquid.Repository.Mongo.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Liquid.Repository.Mongo.Tests
{
    [ExcludeFromCodeCoverage]
    public class IServiceCollectionExtensionsTests
    {
        private IServiceCollection _services;
        private IServiceProvider _serviceProvider;
        private IConfiguration _mongoEntityOptions;
        private MongoDbRunner _runner;

        [SetUp]
        public void Setup()
        {
            _runner = MongoDbRunner.Start(singleNodeReplSet: true);

            var settings = new List<DatabaseSettings>() {
                new DatabaseSettings()
                {
                    ConnectionString = _runner.ConnectionString,
                    DatabaseName = "TestDatabase",

                }
            };

            var mongoSettings = Substitute.For<MongoSettings>();

            mongoSettings.DbSettings = settings;

            var configuration = Substitute.For<ILiquidConfiguration<MongoSettings>>();

            configuration.Settings.Returns(mongoSettings);



            _services = new ServiceCollection();

            _services.AddSingleton(Substitute.For<ILogger<LiquidTelemetryInterceptor>>());

            _services.AddSingleton(configuration);



            var mongoEntityConfiguration = new Dictionary<string, string>
            {
                {"MongoEntityOptions:TestEntity:DatabaseName", "TestDatabase"},
                {"MongoEntityOptions:TestEntity:CollectionName", "TestEntities"},
                {"MongoEntityOptions:TestEntity:ShardKey", "id"},
                {"MongoEntityOptions:AnotherTestEntity:DatabaseName", "TestDatabase"},
                {"MongoEntityOptions:AnotherTestEntity:CollectionName", "AnotherTestEntities"},
                {"MongoEntityOptions:AnotherTestEntity:ShardKey", "id"}
            };

            _mongoEntityOptions = new ConfigurationBuilder()
                                        .AddInMemoryCollection(mongoEntityConfiguration)
                                        .Build()
                                        .GetSection("MongoEntityOptions");
        }

        [TearDown]
        public void DisposeResources()
        {
            _mongoEntityOptions = null;
            _serviceProvider = null;
            _services = null;
            _runner.Dispose();
            _runner = null;
        }

        [Test]
        public void AddLiquidMongoWithTelemetry_WhenMongoEntityOptionsIsNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => _services.AddLiquidMongoWithTelemetry<TestEntity, int>(null));
        }

        [Test]
        public void AddLiquidMongoWithTelemetry_WhenAdded_ServicesIsFilledForTestEntity()
        {
            _services.AddLiquidMongoWithTelemetry<TestEntity, int>(options => { options.DatabaseName = "TestDatabase"; options.CollectionName = "TestEntities"; options.ShardKey = "id"; });
            _serviceProvider = _services.BuildServiceProvider();
            Assert.IsNotNull(_serviceProvider.GetService<IMongoDataContext<TestEntity>>());
            Assert.IsNotNull(_serviceProvider.GetService<ILiquidRepository<TestEntity, int>>());
        }

        [Test]
        public void AddLiquidMongoRepositories_WhenMongoEntityOptionsConfigurationDoesntExist_ThrowsException()
        {
            Assert.Throws<MongoEntityOptionsSettingsDoesNotExistException>(() => _services.AddLiquidMongoRepositories(null));
        }

        [Test]
        public void AddLiquidMongoRepositories_WhenAdded_ServicesIsFilledForTestEntities()
        {
            _services.AddLiquidMongoRepositories(_mongoEntityOptions);
            _serviceProvider = _services.BuildServiceProvider();
            Assert.IsNotNull(_serviceProvider.GetService<IMongoDataContext<TestEntity>>());
            Assert.IsNotNull(_serviceProvider.GetService<ILiquidRepository<TestEntity, int>>());
            Assert.IsNotNull(_serviceProvider.GetService<IMongoDataContext<AnotherTestEntity>>());
            Assert.IsNotNull(_serviceProvider.GetService<ILiquidRepository<AnotherTestEntity, int>>());
        }
    }
}
