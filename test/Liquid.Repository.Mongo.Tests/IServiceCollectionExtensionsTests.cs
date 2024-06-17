using Liquid.Core.Implementations;
using Liquid.Repository.Mongo.Extensions;
using Liquid.Repository.Mongo.Tests.Mock;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EphemeralMongo;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Interfaces;
using Xunit;

namespace Liquid.Repository.Mongo.Tests
{
    [ExcludeFromCodeCoverage]
    public class IServiceCollectionExtensionsTests
    {
        internal const string _databaseName = "TestDatabase";

        private IServiceCollection _services;
        private IServiceProvider _serviceProvider;
        private IConfiguration _configuration;
        private IMongoRunner _runner;

        public IServiceCollectionExtensionsTests()
        {
            var options = new MongoRunnerOptions
            {
                UseSingleNodeReplicaSet = true,
                AdditionalArguments = "--quiet",
                KillMongoProcessesWhenCurrentProcessExits = true
            };

            _runner = MongoRunner.Run(options);

            _services = new ServiceCollection();

            _services.AddSingleton(Substitute.For<ILogger<LiquidTelemetryInterceptor>>());
                        

            var mongoEntityConfiguration = new Dictionary<string, string>
            {
                {"MyMongoEntityOptions:Settings:1:DatabaseName", _databaseName},
                {"MyMongoEntityOptions:Settings:1:ConnectionString", _runner.ConnectionString},
                {"MyMongoEntityOptions:Settings:1:CollectionName", "TestEntity"},
                {"MyMongoEntityOptions:Settings:1:ShardKey", "id"},
                {"MyMongoEntityOptions:Settings:2:DatabaseName", _databaseName},
                {"MyMongoEntityOptions:Settings:2:ConnectionString", _runner.ConnectionString},
                {"MyMongoEntityOptions:Settings:2:CollectionName", "AnotherTestEntity"},
                {"MyMongoEntityOptions:Settings:2:ShardKey", "id"},
            };

            _configuration = new ConfigurationBuilder()
                                        .AddInMemoryCollection(mongoEntityConfiguration).Build();

            _services.AddSingleton<IConfiguration>(_configuration);
        }

        [Fact]
        public void AddLiquidMongoRepository_WhenAdded_ServicesIsFilledForTestEntity()
        {
            _services.AddLiquidMongoRepository<TestEntity, int>("MyMongoEntityOptions","TestEntity");
            _services.AddLiquidMongoRepository<AnotherTestEntity, int>("MyMongoEntityOptions", "AnotherTestEntity");
            _serviceProvider = _services.BuildServiceProvider();
            Assert.NotNull(_serviceProvider.GetService<IMongoDataContext<TestEntity>>());
            Assert.NotNull(_serviceProvider.GetService<ILiquidRepository<TestEntity, int>>());
            Assert.NotNull(_serviceProvider.GetService<IMongoDataContext<AnotherTestEntity>>());
            Assert.NotNull(_serviceProvider.GetService<ILiquidRepository<AnotherTestEntity, int>>());
        }
        
    }
}
