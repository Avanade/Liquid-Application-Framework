using Liquid.Repository.Exceptions;
using MongoDB.Driver;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Liquid.Repository.Mongo.Tests
{
    [ExcludeFromCodeCoverage]
    class MongoDataContextTests
    {
        private MongoDataContext _sut;
        private IMongoClient _client;

        [SetUp]
        protected void SetContext()
        {
            _client = Substitute.For<IMongoClient>();


            var provider = Substitute.For<IMongoClientFactory>();
            provider.GetClient(Arg.Any<string>()).Returns(_client);

            _sut = new MongoDataContext("test", provider);

        }

        [Test]
        public async Task StartTransaction_WhenDBInitialized_Sucess()
        {
            await _sut.StartTransactionAsync();

            await _client.Received(1).StartSessionAsync();

        }

        [Test]
        public async Task CommitAsync_WhenTansactionIsStarted_Sucess()
        {
            await _sut.StartTransactionAsync();

            await _sut.CommitAsync();

            await _sut.ClientSessionHandle.Received().CommitTransactionAsync();

        }

        [Test]
        public void CommitAsync_WhenTansactionIsntStarted_ThrowException()
        {
            var task = _sut.CommitAsync();

            Assert.ThrowsAsync<NullReferenceException>(() => task);
        }

        [Test]
        public async Task RollbackAsync_WhenTansactionIsStarted_Sucess()
        {

            await _sut.StartTransactionAsync();
            await _sut.RollbackTransactionAsync();

            await _sut.ClientSessionHandle.Received().AbortTransactionAsync();

        }

        [Test]
        public void RollbackAsync_WhenTansactionIsntStarted_ThrowException()
        {
            var task = _sut.RollbackTransactionAsync();

            Assert.ThrowsAsync<NullReferenceException>(() => task);

        }
        [Test]
        public async Task Dispose_WhenTansactionIsStarted_Sucess()
        {
            await _sut.StartTransactionAsync();
            _sut.ClientSessionHandle.IsInTransaction.Returns(true);

            _sut.Dispose();

            _sut.ClientSessionHandle.Received().AbortTransaction();
            _sut.ClientSessionHandle.Received().Dispose();

        }

        [Test]
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
