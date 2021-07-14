using Liguid.Repository.Configuration;
using Liquid.Core.Interfaces;
using Liquid.Repository.Configuration;
using Liquid.Repository.Mongo.Configuration;
using Mongo2Go;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Repository.Mongo.Tests
{
    [ExcludeFromCodeCoverage]
    public class MongoClientFactoryTests
    {
        private IMongoClientFactory _sut;
        internal static MongoDbRunner _runner;
        internal static string _databaseName = "IntegrationTest";
        private ILiquidConfiguration<MongoSettings> _configuration;

        [SetUp]
        protected void SetContext()
        {
            _runner = MongoDbRunner.StartForDebugging(singleNodeReplSet: false);

            var settings = new List<DatabaseSettings>() {
                new DatabaseSettings()
                {
                    ConnectionString = _runner.ConnectionString,
                    DatabaseName = _databaseName
                }
            };
            var mongoSettings = Substitute.For<MongoSettings>();

            mongoSettings.DbSettings = settings;

            _configuration = Substitute.For<ILiquidConfiguration<MongoSettings>>();

            _configuration.Settings.Returns(mongoSettings);     

            _sut = new MongoClientFactory(_configuration);

        }

        [Test]
        public void GetClient_WhenDatabaseIdExists_ClientCreated()
        {
            var result = _sut.GetClient("IntegrationTest");

            Assert.IsFalse(result.GetDatabase(_databaseName) is null);
        }

        [Test]
        public void GetClient_WhenDatabaseIdDoesntExists_ThrowException()
        {
            Assert.Throws<LiquidDatabaseSettingsDoesNotExistException>(() => _sut.GetClient("test"));
        }
    }
}
