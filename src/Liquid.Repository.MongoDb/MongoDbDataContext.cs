using Liquid.Core.Telemetry;
using Liquid.Repository.Extensions;
using Liquid.Repository.MongoDb.Exceptions;
using MongoDB.Driver;
using System;
using System.Linq;
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
        private readonly string _connectionString;
        private readonly string _databaseName;
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
        /// <param name="dataContextId">The data context identifier.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="databaseName">Name of the database.</param>
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
        public MongoDbDataContext(ILightTelemetryFactory telemetryFactory,
                                  string dataContextId,
                                  string connectionString,
                                  string databaseName)
        {
            _telemetryFactory = telemetryFactory ?? throw new ArgumentNullException(nameof(telemetryFactory));
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));
            if (string.IsNullOrWhiteSpace(databaseName)) throw new ArgumentNullException(nameof(databaseName));

            _connectionString = connectionString;
            _databaseName = databaseName;
            Id = dataContextId;
            InitializeMongoDbConnection();
        }

        /// <summary>
        /// Initializes the mongo database connection.
        /// </summary>
        /// <exception cref="Liquid.Repository.MongoDb.Exceptions.MongoDatabaseDoesNotExistException"></exception>
        private void InitializeMongoDbConnection()
        {
            if (_mongoClient != null) return;

            _mongoClient = new MongoClient(_connectionString);

            var databaseNames = _mongoClient.ListDatabaseNames().ToList();
            if (!databaseNames.Any(db => db.Equals(_databaseName, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new MongoDatabaseDoesNotExistException(_databaseName);
            }

            _database = _mongoClient.GetDatabase(_databaseName);
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
    }
}
