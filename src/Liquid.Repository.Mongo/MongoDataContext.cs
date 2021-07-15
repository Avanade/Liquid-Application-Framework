using Liquid.Repository.Mongo.Attributes;
using Liquid.Repository.Mongo.Exceptions;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Liquid.Repository.Mongo
{
    /// <summary>
    /// Implements the Mongo data context for repositories.
    /// </summary>
    /// <seealso cref="IMongoDataContext{TEntity}" />
    public class MongoDataContext<TEntity> : IMongoDataContext<TEntity>
    {
        private bool _disposed = false;
        private readonly IMongoClient _mongoClient;
        private IMongoDatabase _database;
        private IClientSessionHandle _clientSessionHandle;
        private readonly MongoAttribute _settings;
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
        /// 
        /// </summary>
        public MongoAttribute Settings => _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDataContext{Tentity}" /> class.
        /// </summary>
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
        public MongoDataContext(IMongoClientFactory clientProvider)
        {    
            if (clientProvider is null) throw new ArgumentNullException(nameof(clientProvider));

            if (!typeof(TEntity).GetCustomAttributes(typeof(MongoAttribute), true).Any())
            {
                throw new NotImplementedException($"The {nameof(MongoAttribute)} attribute decorator must be added to class.");
            }

            _settings = typeof(TEntity).GetCustomAttribute<MongoAttribute>(true);

            _mongoClient = clientProvider.GetClient(_settings.DatabaseName);

            SetDatabase(_settings.DatabaseName);

            ValidateCollection();
        }

        /// <summary>
        /// Starts the transaction of all data contexts in repositories inside UnitOfWork.
        /// </summary>
        public async Task StartTransactionAsync()
        {
            _clientSessionHandle = await _mongoClient.StartSessionAsync();
            _clientSessionHandle.StartTransaction();
        }

        /// <summary>
        /// Commits all commands added to the database context.
        /// </summary>
        public async Task CommitAsync()
        {
            await _clientSessionHandle.CommitTransactionAsync();
        }

        /// <summary>
        /// Rollbacks the transactions.
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            await _clientSessionHandle.AbortTransactionAsync();
        }

        ///<inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the allocated resources <see cref="IClientSessionHandle"/> for this context.
        /// </summary>
        /// <param name="disposing">Indicates if method should perform dispose.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (_clientSessionHandle?.IsInTransaction == true) _clientSessionHandle.AbortTransaction();
                _clientSessionHandle?.Dispose();
            }

            _disposed = true;
        }
        ///<inheritdoc/>
        public void SetDatabase(string databaseName)
        {
            _database = _mongoClient.GetDatabase(databaseName);
        }

        /// <summary>
        /// Validates if the collection exists, if not, an exception is thrown.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MongoCollectionDoesNotExistException">Collection, _databaseName</exception>
        private void ValidateCollection()
        {

            var collection = Database.GetCollection<TEntity>(_settings.CollectionName);
            if (collection is null)
            {
                throw new MongoCollectionDoesNotExistException(_settings.CollectionName, _settings.DatabaseName);
            }
        }
    }
}
