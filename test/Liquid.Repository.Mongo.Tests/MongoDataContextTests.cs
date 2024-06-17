using Liquid.Core.Settings;
using Liquid.Repository.Mongo.Configuration;
using Liquid.Repository.Mongo.Tests.Mock;
using MongoDB.Driver;
using NSubstitute;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Repository.Mongo.Tests
{
    [ExcludeFromCodeCoverage]
    public class MongoDataContextTests
    {
        private MongoDataContext<TestEntity> _sut;
        private IMongoClient _client;
        private IMongoClientFactory _provider;

        public MongoDataContextTests()
        {
            _client = Substitute.For<IMongoClient>();

            var _options = new MongoEntitySettings()
            {
                CollectionName = "TestEntities",
                ShardKey = "id",
                ConnectionString = "test connection string",
                DatabaseName = "TestDatabase"

            };           

            _provider = Substitute.For<IMongoClientFactory>();

            _provider.GetClient("TestEntities", out _).Returns(x => { x[1] = _options; return _client; });

            _sut = new MongoDataContext<TestEntity>(_provider, "TestEntities");
        }

        [Fact]
        public void MongoDataContext_WhenCreatedWithNullArguments_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new MongoDataContext<TestEntity>(null, "TestEntities"));
            Assert.Throws<ArgumentNullException>(() => new MongoDataContext<TestEntity>(_provider, null));
        }


        [Fact]
        public async Task StartTransaction_WhenDBInitialized_Sucess()
        {
            await _sut.StartTransactionAsync();

            await _client.Received(1).StartSessionAsync();

        }

        [Fact]
        public async Task CommitAsync_WhenTansactionIsStarted_Sucess()
        {
            await _sut.StartTransactionAsync();

            await _sut.CommitAsync();

            await _sut.ClientSessionHandle.Received().CommitTransactionAsync();

        }

        [Fact]
        public async Task CommitAsync_WhenTansactionIsntStarted_ThrowException()
        {
            var task = _sut.CommitAsync();

            await Assert.ThrowsAsync<NullReferenceException>(() => task);
        }

        [Fact]
        public async Task RollbackAsync_WhenTansactionIsStarted_Sucess()
        {

            await _sut.StartTransactionAsync();
            await _sut.RollbackTransactionAsync();

            await _sut.ClientSessionHandle.Received().AbortTransactionAsync();

        }

        [Fact]
        public async Task RollbackAsync_WhenTansactionIsntStarted_ThrowException()
        {
            var task = _sut.RollbackTransactionAsync();

            await Assert.ThrowsAsync<NullReferenceException>(() => task);

        }

        [Fact]
        public async Task Dispose_WhenTansactionIsStarted_Sucess()
        {
            await _sut.StartTransactionAsync();
            _sut.ClientSessionHandle.IsInTransaction.Returns(true);

            _sut.Dispose();

            _sut.ClientSessionHandle.Received().AbortTransaction();
            _sut.ClientSessionHandle.Received().Dispose();

        }

        [Fact]
        public async Task Dispose_WhenTansactionIsntStarted_HandlerDisposed()
        {
            await _sut.StartTransactionAsync();
            _sut.ClientSessionHandle.IsInTransaction.Returns(false);

            _sut.Dispose();

            _sut.ClientSessionHandle.DidNotReceive().AbortTransaction();
            _sut.ClientSessionHandle.Received().Dispose();

        }

    }
}
