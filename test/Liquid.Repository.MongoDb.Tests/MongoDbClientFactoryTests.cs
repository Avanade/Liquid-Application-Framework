using Liguid.Repository.Configuration;
using Liquid.Core.Configuration;
using Mongo2Go;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace Liquid.Repository.MongoDb.Tests
{
    public class MongoDbClientFactoryTests
    {
        private IMongoDbClientFactory _sut;
        internal static MongoDbRunner _runner;
        internal static string _databaseName = "IntegrationTest";
        private ILightConfiguration<List<LightConnectionSettings>> _configuration;

        [SetUp]
        protected void SetContext()
        {
            _runner = MongoDbRunner.StartForDebugging(singleNodeReplSet: false);

            var connectionSettings = new LightConnectionSettings()
            {
                Id = "test",
                ConnectionString = _runner.ConnectionString,
                DatabaseName = _databaseName
            };
            _configuration = Substitute.For<ILightConfiguration<List<LightConnectionSettings>>>();

            _configuration.Settings
                .Returns(new List<LightConnectionSettings>() { connectionSettings }, new List<LightConnectionSettings>() { connectionSettings });

            _sut = new MongoDbClientFactory(_configuration);

        }

        [Test]
        public void GetClient_WhenDatabaseNameExists_ClientCreated()
        {
            var result = _sut.GetClient("test");

            Assert.IsFalse(result.GetDatabase(_databaseName) is null);
        }
    }
}
