using EphemeralMongo;
using Liquid.Core.Settings;
using Liquid.Repository.Mongo.Settings;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Liquid.Repository.Mongo.Tests
{
    [ExcludeFromCodeCoverage]
    public class MongoClientFactoryTests
    {
        private IMongoClientFactory _sut;
        internal static IMongoRunner _runner;
        internal const string _databaseName = "TestDatabase";
        private IOptions<MongoDbSettings> _options;
        public MongoClientFactoryTests()
        {
            var options = new MongoRunnerOptions
            {
                UseSingleNodeReplicaSet = false,
                AdditionalArguments = "--quiet"
            };

            _runner = MongoRunner.Run(options);


            _options = Substitute.For<IOptions<MongoDbSettings>>();

            var settings = new MongoDbSettings()
            {
                Settings = new System.Collections.Generic.List<MongoEntitySettings>()
                {
                    new MongoEntitySettings()
                    {
                        CollectionName = "TestEntities",
                        ShardKey = "id",
                        ConnectionString = _runner.ConnectionString,
                        DatabaseName = _databaseName
                    },
                    new MongoEntitySettings()
                    {
                        CollectionName = "TestEntities2",
                        ShardKey = "id",
                        ConnectionString = "incorrect connection string",
                        DatabaseName = $"{_databaseName}-2"
                    }
                }
            };

            _options.Value.Returns(settings);

            _sut = new MongoClientFactory(_options);
        }


        [Fact]
        public void MongoClientFactory_WhenSettingsIsNull_ThrowException()
        {
            MongoEntitySettings settings = null;
            Assert.Throws<ArgumentNullException>(() => _sut.GetClient(null, out settings));
        }

        [Fact]
        public void GetClient_WhenDatabaseIdsExists_ClientCreated()
        {
            MongoEntitySettings settings = null;
            var result = _sut.GetClient("TestEntities", out settings);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetClient_WhenDatabaseSettingsIsWrong_ThrowException()
        {
            MongoEntitySettings settings = null;
            Assert.Throws<MongoDB.Driver.MongoConfigurationException>(() => _sut.GetClient("TestEntities2", out settings));
        }
    }
}
