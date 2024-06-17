using Liquid.Repository.Mongo.Configuration;
using Liquid.Repository.Mongo.Exceptions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
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
        private readonly MongoEntitySettings _settings;

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
        public MongoEntitySettings Settings => _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDataContext{Tentity}" /> class.
        /// </summary>
        /// <param name="clientProvider">Mongo client generator.</param>
        /// <param name="collectionName">        /// </param>
        /// <exception cref="ArgumentNullException">
        /// clientProvider
        /// or
        /// settingsFactory
        /// </exception>
        public MongoDataContext(IMongoClientFactory clientProvider, string collectionName)
        {    
            if (clientProvider is null) throw new ArgumentNullException(nameof(clientProvider));
            if (collectionName is null) throw new ArgumentNullException(nameof(collectionName));                      
                        
            _mongoClient = clientProvider.GetClient(collectionName, out _settings);

            if (_settings is null) throw new MongoEntitySettingsDoesNotExistException(nameof(TEntity));

            SetDatabase(_settings.DatabaseName);
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
    }
}
