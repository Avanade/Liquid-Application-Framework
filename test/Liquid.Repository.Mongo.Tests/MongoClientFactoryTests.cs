using Liguid.Repository.Configuration;
using Liquid.Core.Configuration;
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
        private ILightDatabaseConfiguration<MongoSettings> _configuration;

        [SetUp]
        protected void SetContext()
        {
            _runner = MongoDbRunner.StartForDebugging(singleNodeReplSet: false);

            var connectionSettings = new MongoSettings()
            {
                ConnectionString = _runner.ConnectionString,
                DatabaseName = _databaseName
            };
            _configuration = Substitute.For<ILightDatabaseConfiguration<MongoSettings>>();

            _configuration.GetSettings("test").Returns(connectionSettings);

            _sut = new MongoClientFactory(_configuration);

        }

        [Test]
        public void GetClient_WhenDatabaseIdExists_ClientCreated()
        {
            var result = _sut.GetClient("test");

            Assert.IsFalse(result.GetDatabase(_databaseName) is null);
        }
        [Test]
        public void GetClient_WhenDatabaseIdDoesntExists_ThrowException()
        {
            Assert.Throws<LightDatabaseConfigurationDoesNotExistException>(() => _sut.GetClient("test1"));
        }
    }
}
