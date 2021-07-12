using Liquid.Repository.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Liquid.Repository.EntityFramework.Tests
{
    public class EntityFrameworkDataContextTests
    {
        private EntityFrameworkDataContext<DbContext> _sut;
        private DbContext _client;
        private DatabaseFacade _database;

        [SetUp]
        protected void SetContext()
        {

            _client = Substitute.For<DbContext>();

            _database = Substitute.For<DatabaseFacade>(_client);

            _client.Database.Returns(_database);

            _sut = new EntityFrameworkDataContext<DbContext>( _client);
        }

        [Test]
        public async Task StartTransactionAsync_WhenClientExecutedSucessfuly_Success()
        {
            await _sut.StartTransactionAsync();

            await _client.Database.Received(1).BeginTransactionAsync();
        }

        [Test]
        public void StartTransactionAsync_WhenClientThrow_ThrowException()
        {
            _database.When(o => o.BeginTransactionAsync()).Do((call) => throw new Exception());

            var task = _sut.StartTransactionAsync();

            Assert.ThrowsAsync<Exception>(() => task);
        }

        [Test]
        public async Task CommitAsync_WhenClientExecutedSucessfuly_Success()
        {
            await _sut.CommitAsync();

            await _client.Database.Received(1).CommitTransactionAsync();
        }

        [Test]
        public void CommitAsync_WhenClientExcept_ThrowException()
        {
            _database.When(o => o.CommitTransactionAsync()).Do((call) => throw new Exception());

            var task = _sut.CommitAsync();

            Assert.ThrowsAsync<Exception>(() => task);
        }

        [Test]
        public async Task RollbackTransactionAsync_WhenClientExecutedSucessfuly_Success()
        {
            await _sut.RollbackTransactionAsync();

            await _client.Database.Received(1).RollbackTransactionAsync();
        }

        [Test]
        public void RollbackTransactionAsync_WhenClientExcept_ThrowException()
        {
            _database.When(o => o.RollbackTransactionAsync()).Do((call) => throw new Exception());

            var task = _sut.RollbackTransactionAsync();

            Assert.ThrowsAsync<Exception>(() => task);
        }

        [Test]
        public void Verify_Dispose()
        {
            _sut.Dispose();

            _client.Received(1).Dispose();
        }

        [Test]
        public void Verify_Dispose_Except()
        {
            _client.When(o => o.Dispose()).Do((call) => throw new Exception());

            Assert.Throws<Exception>(() => _sut.Dispose());
        }
    }
}
