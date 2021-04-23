using Liquid.Core.Telemetry;
using Liquid.Repository.Extensions;
using Liquid.Repository.MongoDb.Attributes;
using Liquid.Repository.MongoDb.Exceptions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Liquid.Repository.MongoDb
{
    /// <summary>
    /// Implements the EntityFramework repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TIdentifier">The type of the identifier.</typeparam>
    /// <seealso cref="Liquid.Repository.ILightRepository{TEntity, TIdentifier}" />
    public abstract class MongoDbRepository<TEntity, TIdentifier> : ILightRepository<TEntity, TIdentifier> where TEntity : RepositoryEntity<TIdentifier>, new()
    {
        private readonly ILightTelemetryFactory _telemetryFactory;
        private readonly MongoDbAttribute _mongoDbAttribute;

        ///<inheritdoc/>
        public IMongoDbDataContext MongoDataContext { get; }

        ///<inheritdoc/>
        public ILightDataContext DataContext => MongoDataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbRepository{TEntity, TIdentifier}" /> class.
        /// </summary>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="dataContext">The data context.</param>
        /// <exception cref="System.ArgumentNullException">
        /// telemetryFactory
        /// or
        /// dataContext
        /// </exception>
        protected MongoDbRepository(ILightTelemetryFactory telemetryFactory,
                                    IMongoDbDataContext dataContext)
        {
            _telemetryFactory = telemetryFactory ?? throw new ArgumentNullException(nameof(telemetryFactory));
            MongoDataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            if (!GetType().GetCustomAttributes(typeof(MongoDbAttribute), true).Any())
            {
                throw new NotImplementedException($"The {nameof(MongoDbAttribute)} attribute decorator must be added to class.");
            }
            _mongoDbAttribute = GetType().GetCustomAttribute<MongoDbAttribute>(true);

            if (!BsonClassMap.IsClassMapRegistered(typeof(TEntity)))
            {
                BsonClassMap.RegisterClassMap<TEntity>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }

            MongoDataContext.SetDatabase(_mongoDbAttribute.DatabaseName);

            ValidateCollection();
        }

        /// <summary>
        /// Adds the specified entity item in repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task AddAsync(TEntity entity)
        {
            await _telemetryFactory.ExecuteActionAsync("MongoDbRepository_AddAsync", async () =>
                {
                    var collection = MongoDataContext.Database.GetCollection<TEntity>(_mongoDbAttribute.CollectionName);
                    await collection.InsertOneAsync(entity);
                });
        }

        /// <summary>
        /// Retrieve all entities.
        /// </summary>
        /// <returns>
        /// List of all entities.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            IEnumerable<TEntity> returnValue = null;

            await _telemetryFactory.ExecuteActionAsync("MongoDbRepository_GetAllAsync", async () =>
                {
                    var collection = MongoDataContext.Database.GetCollection<TEntity>(_mongoDbAttribute.CollectionName);
                    returnValue = (await collection.FindAsync(new BsonDocument())).Current.AsEnumerable();
                });

            return returnValue;
        }

        /// <summary>
        /// Finds the entity by identifier.
        /// </summary>
        /// <param name="id">The entity identifier.</param>
        /// <returns>
        /// The entity
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<TEntity> FindByIdAsync(TIdentifier id)
        {
            TEntity returnValue = null;

            await _telemetryFactory.ExecuteActionAsync("MongoDbRepository_FindByIdAsync", async () =>
            {
                var collection = MongoDataContext.Database.GetCollection<TEntity>(_mongoDbAttribute.CollectionName);
                var result = await collection.FindAsync(e => e.Id.Equals(id));
                returnValue = result.SingleOrDefault();
            });

            return returnValue;
        }

        /// <summary>
        /// Removes the specified entity item from repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task RemoveAsync(TEntity entity)
        {
            await _telemetryFactory.ExecuteActionAsync("MongoDbRepository_RemoveAsync", async () =>
            {
                var collection = MongoDataContext.Database.GetCollection<TEntity>(_mongoDbAttribute.CollectionName);
                await collection.DeleteOneAsync(e => e.Id.Equals(entity.Id));
            });
        }

        /// <summary>
        /// Updates the specified entity item in repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task UpdateAsync(TEntity entity)
        {
            await _telemetryFactory.ExecuteActionAsync("MongoDbRepository_RemoveAsync", async () =>
            {
                var collection = MongoDataContext.Database.GetCollection<TEntity>(_mongoDbAttribute.CollectionName);
                await collection.ReplaceOneAsync(x => x.Id.Equals(entity.Id), entity, new ReplaceOptions { IsUpsert = true });
            });
        }

        /// <summary>
        /// Finds the specified entity by the search predicate.
        /// </summary>
        /// <param name="whereClause">The search predicate.</param>
        /// <returns>
        /// List of entities.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> whereClause)
        {
            IEnumerable<TEntity> returnValue = null;

            await _telemetryFactory.ExecuteActionAsync("MongoDbRepository_GetAllAsync", async () =>
            {
                var collection = MongoDataContext.Database.GetCollection<TEntity>(_mongoDbAttribute.CollectionName);
                returnValue = (await collection.FindAsync(whereClause)).Current.AsEnumerable();
            });

            return returnValue;
        }

        /// <summary>
        /// Validates if the collection exists, if not, an exception is thrown.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MongoCollectionDoesNotExistException">Collection, _databaseName</exception>
        private void ValidateCollection()
        {

            var collection = MongoDataContext.Database.GetCollection<TEntity>(_mongoDbAttribute.CollectionName);
            if (collection is null)
            {
                throw new MongoCollectionDoesNotExistException(_mongoDbAttribute.CollectionName, MongoDataContext.Database.DatabaseNamespace.DatabaseName);
            }

        }
    }
}
