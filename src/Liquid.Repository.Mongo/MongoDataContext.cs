using Liquid.Core.Telemetry;
using Liquid.Repository.Extensions;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Liquid.Repository.Mongo
{
    /// <summary>
    /// Implements the Mongo data context for repositories.
    /// </summary>
    /// <seealso cref="Liquid.Repository.Mongo.IMongoDataContext" />
    public class MongoDataContext : IMongoDataContext, IDisposable
    {
        private readonly ILightTelemetryFactory _telemetryFactory;
        private IMongoClient _mongoClient;
        private IMongoDatabase _database;
        private IClientSessionHandle _clientSessionHandle;

        /// <summary>
        /// Gets the Mongo Database.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public IMongoDatabase Database => _database;

        /// <summary>
        /// Gets the mongo client.
        /// </summary>
        /// <value>
        /// The mongo client.
        /// </value>
        public IMongoClient MongoClient => _mongoClient;

        /// <summary>
        /// Gets the mongo session handle.
        /// </summary>
        public IClientSessionHandle ClientSessionHandle => _clientSessionHandle;

        /// <summary>
        /// Gets the identifier of data context.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDataContext" /> class.
        /// </summary>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="connectionId">Database connection configuration Id.</param>
        /// <param name="clientProvider">Mongo client generator.</param>
        /// <exception cref="ArgumentNullException">
        /// telemetryFactory
        /// or
        /// connectionString
        /// or
        /// databaseName
        /// </exception>
        /// <exception cref="System.ArgumentNullException">connectionString
        /// or
        /// databaseName</exception>
        public MongoDataContext(ILightTelemetryFactory telemetryFactory, string connectionId, IMongoClientFactory clientProvider)
        {
            _telemetryFactory = telemetryFactory ?? throw new ArgumentNullException(nameof(telemetryFactory));
            if (connectionId is null) throw new ArgumentNullException(nameof(connectionId));
            if (clientProvider is null) throw new ArgumentNullException(nameof(clientProvider));

            _mongoClient = clientProvider.GetClient(connectionId);
        }

        /// <summary>
        /// Starts the transaction of all data contexts in repositories inside UnitOfWork.
        /// </summary>
        public async Task StartTransactionAsync()
        {
            await _telemetryFactory.ExecuteActionAsync("MongoRepository_StartTransactionAsync", async () =>
            {
                _clientSessionHandle = await _mongoClient.StartSessionAsync();
                _clientSessionHandle.StartTransaction();
            });
        }

        /// <summary>
        /// Commits all commands added to the database context.
        /// </summary>
        public async Task CommitAsync()
        {
            await _telemetryFactory.ExecuteActionAsync("MongoRepository_CommitAsync", async () =>
            {
                await _clientSessionHandle.CommitTransactionAsync();
            });
        }

        /// <summary>
        /// Rollbacks the transactions.
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            await _telemetryFactory.ExecuteActionAsync("MongoRepository_RollbackTransactionAsync", async () =>
            {
                await _clientSessionHandle.AbortTransactionAsync();
            });
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_clientSessionHandle?.IsInTransaction == true) _clientSessionHandle.AbortTransaction();
            _clientSessionHandle?.Dispose();
        }

        ///<inheritdoc/>
        public void SetDatabase(string databaseName)
        {
            _database = _mongoClient.GetDatabase(databaseName);
        }
    }
}
