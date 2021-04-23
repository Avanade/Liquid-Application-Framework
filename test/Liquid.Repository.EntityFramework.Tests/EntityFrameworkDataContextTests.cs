using Liquid.Core.Telemetry;
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
        private EntityFrameworkDataContext _sut;
        private ILightTelemetryFactory _telemetryFactory;
        private ILightTelemetry _telemetry;
        private IEntityFrameworkClientFactory _clientFactory;
        private DbContext _client;
        private DatabaseFacade _database;

        [SetUp]
        protected void SetContext()
        {
            _telemetry = Substitute.For<ILightTelemetry>();
            _telemetryFactory = Substitute.For<ILightTelemetryFactory>();  
            _telemetryFactory.GetTelemetry().Returns(_telemetry);

            _client = Substitute.For<DbContext>();

            _database = Substitute.For<DatabaseFacade>(_client);


            _client.Database.Returns(_database);
            
            _clientFactory = Substitute.For<IEntityFrameworkClientFactory>();

            _clientFactory.GetClient(Arg.Any<string>()).Returns(_client);

            _sut = new EntityFrameworkDataContext(_telemetryFactory, _clientFactory, "test");
        }

        [Test]
        public async Task StartTransactionAsync_WhenClientExecutedSucessfuly_Success()
        {
            await _sut.StartTransactionAsync();

            _telemetry.Received(1).AddContext(Arg.Any<string>());
            _telemetry.Received(1).RemoveContext(Arg.Any<string>());

            await _client.Database.Received(1).BeginTransactionAsync();
        }

        [Test]
        public void StartTransactionAsync_WhenClientThrow_ThrowException()
        {
            _database.When(o => o.BeginTransactionAsync()).Do((call) => throw new Exception());

            var task = _sut.StartTransactionAsync();

            Assert.ThrowsAsync<DatabaseContextException>(() => task);
        }

        [Test]
        public async Task CommitAsync_WhenClientExecutedSucessfuly_Success()
        {
            await _sut.CommitAsync();

            _telemetry.Received(1).AddContext(Arg.Any<string>());
            _telemetry.Received(1).RemoveContext(Arg.Any<string>());

            await _client.Database.Received(1).CommitTransactionAsync();
        }

        [Test]
        public void CommitAsync_WhenClientExcept_ThrowException()
        {
            _database.When(o => o.CommitTransactionAsync()).Do((call) => throw new Exception());

            var task = _sut.CommitAsync();

            Assert.ThrowsAsync<DatabaseContextException>(() => task);
        }

        [Test]
        public async Task RollbackTransactionAsync_WhenClientExecutedSucessfuly_Success()
        {
            await _sut.RollbackTransactionAsync();

            _telemetry.Received(1).AddContext(Arg.Any<string>());
            _telemetry.Received(1).RemoveContext(Arg.Any<string>());

            await _client.Database.Received(1).RollbackTransactionAsync();
        }

        [Test]
        public void RollbackTransactionAsync_WhenClientExcept_ThrowException()
        {
            _database.When(o => o.RollbackTransactionAsync()).Do((call) => throw new Exception());

            var task = _sut.RollbackTransactionAsync();

            Assert.ThrowsAsync<DatabaseContextException>(() => task);
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
