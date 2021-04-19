using Liquid.Core.Telemetry;
using Liquid.Repository.Extensions;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Liquid.Repository.MongoDb
{
    /// <summary>
    /// Implements the MongoDb data context for repositories.
    /// </summary>
    /// <seealso cref="Liquid.Repository.MongoDb.IMongoDbDataContext" />
    public class MongoDbDataContext : IMongoDbDataContext, IDisposable
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
        /// Gets the identifier of data context.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbDataContext" /> class.
        /// </summary>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="connectionId">Database connection configuration Id.</param>
        /// <param name="clientProvider">MongoDb client generator.</param>
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
        public MongoDbDataContext(ILightTelemetryFactory telemetryFactory, string connectionId, IMongoDbClientFactory clientProvider)
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
            await _telemetryFactory.ExecuteActionAsync("MongoDbRepository_StartTransactionAsync", async () =>
            {
                _clientSessionHandle = await _mongoClient.StartSessionAsync();
            });
        }

        /// <summary>
        /// Commits all commands added to the database context.
        /// </summary>
        public async Task CommitAsync()
        {
            if (_clientSessionHandle.IsInTransaction)
            {
                await _telemetryFactory.ExecuteActionAsync("MongoDbRepository_CommitAsync", async () =>
                {
                    await _clientSessionHandle.CommitTransactionAsync();
                });
            }
        }

        /// <summary>
        /// Rollbacks the transactions.
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            if (_clientSessionHandle.IsInTransaction)
            {
                await _telemetryFactory.ExecuteActionAsync("MongoDbRepository_RollbackTransactionAsync", async () =>
                {
                    await _clientSessionHandle.AbortTransactionAsync();
                });
            }
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
