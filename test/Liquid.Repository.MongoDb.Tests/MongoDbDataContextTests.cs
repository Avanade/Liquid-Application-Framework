using Liquid.Core.Telemetry;
using MongoDB.Driver;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Liquid.Repository.MongoDb.Tests
{
    class MongoDbDataContextTests
    {
        private MongoDbDataContext _sut;
        private IMongoClient _client;
        private ILightTelemetryFactory _telemetryFactory;
        private ILightTelemetry _lightTelemetry;

        [SetUp]
        protected void SetContext()
        {
            _client = Substitute.For<IMongoClient>();

            _telemetryFactory = Substitute.For<ILightTelemetryFactory>();

            _lightTelemetry = Substitute.For<ILightTelemetry>();

            _telemetryFactory.GetTelemetry().Returns(_lightTelemetry);

            var provider = Substitute.For<IMongoDbClientFactory>();
            provider.GetClient(Arg.Any<string>()).Returns(_client);

            _sut = new MongoDbDataContext(_telemetryFactory
                , "test", provider);

        }

        [Test]
        public async Task StartTransaction_WhenDBInitialized_Sucess()
        {
            await _sut.StartTransactionAsync();

            await _client.Received(1).StartSessionAsync();

            _lightTelemetry.Received().AddContext(Arg.Any<string>());
            _lightTelemetry.Received().RemoveContext(Arg.Any<string>());
        }

        [Test]
        public async Task CommitAsync_WhenTansactionIsStarted_Sucess()
        {
            IClientSessionHandle sessionHandler = GetSessionhandler(true);

            await _sut.StartTransactionAsync();
            await _sut.CommitAsync();

            await sessionHandler.Received().CommitTransactionAsync();

            _lightTelemetry.Received().AddContext(Arg.Any<string>());
            _lightTelemetry.Received().RemoveContext(Arg.Any<string>());

        }

        private IClientSessionHandle GetSessionhandler(bool isInTtransaction)
        {
            var sessionHandler = Substitute.For<IClientSessionHandle>();
            _client.StartSessionAsync().Returns(sessionHandler);
            sessionHandler.IsInTransaction.Returns(isInTtransaction);
            return sessionHandler;
        }

        [Test]
        public async Task CommitAsync_WhenTansactionIsntStarted_ClientNotExecuted()
        {
            IClientSessionHandle sessionHandler = GetSessionhandler(false);

            await _sut.StartTransactionAsync();
            await _sut.CommitAsync();

            await sessionHandler.DidNotReceive().CommitTransactionAsync();

        }
        [Test]
        public async Task RollbackAsync_WhenTansactionIsStarted_Sucess()
        {
            IClientSessionHandle sessionHandler = GetSessionhandler(true);

            await _sut.StartTransactionAsync();
            await _sut.RollbackTransactionAsync();

            await sessionHandler.Received().AbortTransactionAsync();

            _lightTelemetry.Received().AddContext(Arg.Any<string>());
            _lightTelemetry.Received().RemoveContext(Arg.Any<string>());

        }

        [Test]
        public async Task RollbackAsync_WhenTansactionIsntStarted_ClientNotExecuted()
        {
            IClientSessionHandle sessionHandler = GetSessionhandler(false);

            await _sut.StartTransactionAsync();
            await _sut.RollbackTransactionAsync();

            await sessionHandler.DidNotReceive().AbortTransactionAsync();

        }
        [Test]
        public async Task Dispose_WhenTansactionIsStarted_Sucess()
        {
            IClientSessionHandle sessionHandler = GetSessionhandler(true);

            await _sut.StartTransactionAsync();
            _sut.Dispose();

            sessionHandler.Received().AbortTransaction();
            sessionHandler.Received().Dispose();

        }

        [Test]
        public async Task Dispose_WhenTansactionIsntStarted_HandlerDisposed()
        {
            IClientSessionHandle sessionHandler = GetSessionhandler(false);

            await _sut.StartTransactionAsync();
            _sut.Dispose();

            sessionHandler.DidNotReceive().AbortTransaction();
            sessionHandler.Received().Dispose();

        }

    }
}
