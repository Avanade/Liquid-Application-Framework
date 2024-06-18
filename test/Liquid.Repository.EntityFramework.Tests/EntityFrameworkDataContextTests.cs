using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NSubstitute;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Repository.EntityFramework.Tests
{
    [ExcludeFromCodeCoverage]
    public class EntityFrameworkDataContextTests
    {
        private EntityFrameworkDataContext<DbContext> _sut;
        private DbContext _client;
        private DatabaseFacade _database;

        
        public EntityFrameworkDataContextTests()
        {

            _client = Substitute.For<DbContext>();

            _database = Substitute.For<DatabaseFacade>(_client);

            _client.Database.Returns(_database);

            _sut = new EntityFrameworkDataContext<DbContext>(_client);
        }

        [Fact]
        public async Task StartTransactionAsync_WhenClientExecutedSucessfuly_Success()
        {
            await _sut.StartTransactionAsync();

            await _client.Database.Received(1).BeginTransactionAsync();
        }

        [Fact]
        public async Task StartTransactionAsync_WhenClientThrow_ThrowException()
        {
            _database.When(o => o.BeginTransactionAsync()).Do((call) => throw new Exception());

            var task = _sut.StartTransactionAsync();

            await Assert.ThrowsAsync<Exception>(() => task);
        }

        [Fact]
        public async Task CommitAsync_WhenClientExecutedSucessfuly_Success()
        {
            await _sut.CommitAsync();

            await _client.Database.Received(1).CommitTransactionAsync();
        }

        [Fact]
        public async Task CommitAsync_WhenClientExcept_ThrowException()
        {
            _database.When(o => o.CommitTransactionAsync()).Do((call) => throw new Exception());

            var task = _sut.CommitAsync();

            await Assert.ThrowsAsync<Exception>(() => task);
        }

        [Fact]
        public async Task RollbackTransactionAsync_WhenClientExecutedSucessfuly_Success()
        {
            await _sut.RollbackTransactionAsync();

            await _client.Database.Received(1).RollbackTransactionAsync();
        }

        [Fact]
        public async Task RollbackTransactionAsync_WhenClientExcept_ThrowException()
        {
            _database.When(o => o.RollbackTransactionAsync()).Do((call) => throw new Exception());

            var task = _sut.RollbackTransactionAsync();

            await Assert.ThrowsAsync<Exception>(() => task);
        }

        [Fact]
        public void Verify_Dispose()
        {
            _sut.Dispose();

            _client.Received(1).Dispose();

            // Try to dispose twice to cover all conditions on Dispose method
            _sut.Dispose();
        }

        [Fact]
        public void Verify_Dispose_Except()
        {
            _client.When(o => o.Dispose()).Do((call) => throw new Exception());

            Assert.Throws<Exception>(() => _sut.Dispose());
        }

        [Fact]
        public void EntityFrameworkDataContext_WhenCreated_DbContextIsValid()
        {
            Assert.NotNull(_sut.DbClient);
            Assert.IsAssignableFrom<DbContext>(_sut.DbClient);
        }

        [Fact]
        public void EntityFrameworkDataContext_WhenCreatedWithoutDbContext_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new EntityFrameworkDataContext<DbContext>(null));
        }

        [Fact]
        public void EntityFrameworkDataContext_IdIsAlwaysNull()
        {
            Assert.Null(_sut.Id);
        }
    }
}
