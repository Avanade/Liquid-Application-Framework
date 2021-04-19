using Liguid.Repository.Configuration;
using Mongo2Go;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Liquid.Repository.MongoDb.Tests
{
    public class MongoDbClientFactoryTests
    {
        private IMongoDbClientFactory _sut;
        internal static MongoDbRunner _runner;
        internal static string _databaseName = "IntegrationTest";
        [SetUp]
        protected void SetContext()
        {
            _runner = MongoDbRunner.StartForDebugging(singleNodeReplSet: false);

            _sut = new MongoDbClientFactory();

        }

        [Test]
        public async Task GetClient_WhenDatabaseNameExists_ClientCreated()
        {
            var connectionSettings = new LightConnectionSettings()
            {
                ConnectionString = _runner.ConnectionString,
                DatabaseName = _databaseName
            };

            var result = _sut.GetClient(connectionSettings);

            Assert.IsFalse(result.GetDatabase(_databaseName) is null);
        }
    }
}
