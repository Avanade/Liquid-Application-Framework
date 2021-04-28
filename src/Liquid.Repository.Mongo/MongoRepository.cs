using Liquid.Core.Telemetry;
using Liquid.Repository.Extensions;
using Liquid.Repository.Mongo.Attributes;
using Liquid.Repository.Mongo.Exceptions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Liquid.Repository.Mongo.Extensions;

namespace Liquid.Repository.Mongo
{
    /// <summary>
    /// Mongo database repository class. Implements the <seealso cref="ILightRepository{TEntity, TIdentifier}"/> interface to provide 
    /// the repository pattern access to a Mongo Db document. Also provides a Mongo data context to extend Mongo client resources.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TIdentifier">The type of the identifier.</typeparam>
    /// <seealso cref="Liquid.Repository.ILightRepository{TEntity, TIdentifier}" />
    public abstract class MongoRepository<TEntity, TIdentifier> : ILightRepository<TEntity, TIdentifier> where TEntity : RepositoryEntity<TIdentifier>, new()
    {
        private readonly ILightTelemetryFactory _telemetryFactory;
        private readonly MongoAttribute _MongoAttribute;

        ///<inheritdoc/>
        public IMongoDataContext MongoDataContext { get; }

        ///<inheritdoc/>
        public ILightDataContext DataContext => MongoDataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoRepository{TEntity, TIdentifier}" /> class.
        /// </summary>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="dataContext">The data context.</param>
        /// <exception cref="System.ArgumentNullException">
        /// telemetryFactory
        /// or
        /// dataContext
        /// </exception>
        protected MongoRepository(ILightTelemetryFactory telemetryFactory,
                                  IMongoDataContext dataContext)
        {
            _telemetryFactory = telemetryFactory ?? throw new ArgumentNullException(nameof(telemetryFactory));
            MongoDataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            if (!GetType().GetCustomAttributes(typeof(MongoAttribute), true).Any())
            {
                throw new NotImplementedException($"The {nameof(MongoAttribute)} attribute decorator must be added to class.");
            }
            _MongoAttribute = GetType().GetCustomAttribute<MongoAttribute>(true);

            if (!BsonClassMap.IsClassMapRegistered(typeof(TEntity)))
            {
                BsonClassMap.RegisterClassMap<TEntity>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }

            MongoDataContext.SetDatabase(_MongoAttribute.DatabaseName);

            ValidateCollection();
        }

        ///<inheritdoc/>
        public async Task AddAsync(TEntity entity)
        {
            await _telemetryFactory.ExecuteActionAsync("MongoRepository_AddAsync", async () =>
                {
                    var collection = MongoDataContext.Database.GetCollection<TEntity>(_MongoAttribute.CollectionName);
                    await collection.InsertOneAsync(entity, MongoDataContext?.ClientSessionHandle);
                });
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            IEnumerable<TEntity> returnValue = null;

            await _telemetryFactory.ExecuteActionAsync("MongoRepository_GetAllAsync", async () =>
                {
                    var collection = MongoDataContext.Database.GetCollection<TEntity>(_MongoAttribute.CollectionName);
                    returnValue = (await collection.FindAsync(new BsonDocument(), MongoDataContext?.ClientSessionHandle)).Current.AsEnumerable();
                });

            return returnValue;
        }

        ///<inheritdoc/>
        public async Task<TEntity> FindByIdAsync(TIdentifier id)
        {
            TEntity returnValue = null;

            await _telemetryFactory.ExecuteActionAsync("MongoRepository_FindByIdAsync", async () =>
            {
                var collection = MongoDataContext.Database.GetCollection<TEntity>(_MongoAttribute.CollectionName);
                var result = await collection.FindAsync(e => e.Id.Equals(id), MongoDataContext?.ClientSessionHandle);
                returnValue = result.SingleOrDefault();
            });

            return returnValue;
        }

        ///<inheritdoc/>
        public async Task RemoveAsync(TEntity entity)
        {
            await _telemetryFactory.ExecuteActionAsync("MongoRepository_RemoveAsync", async () =>
            {
                var collection = MongoDataContext.Database.GetCollection<TEntity>(_MongoAttribute.CollectionName);
                await collection.DeleteOneAsync(e => e.Id.Equals(entity.Id), MongoDataContext?.ClientSessionHandle);
            });
        }

        ///<inheritdoc/>
        public async Task UpdateAsync(TEntity entity)
        {
            await _telemetryFactory.ExecuteActionAsync("MongoRepository_RemoveAsync", async () =>
            {
                var collection = MongoDataContext.Database.GetCollection<TEntity>(_MongoAttribute.CollectionName);
                await collection.ReplaceOneAsync(x => x.Id.Equals(entity.Id), entity, new ReplaceOptions { IsUpsert = true }, MongoDataContext?.ClientSessionHandle);
            });
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> whereClause)
        {
            IEnumerable<TEntity> returnValue = null;

            await _telemetryFactory.ExecuteActionAsync("MongoRepository_GetAllAsync", async () =>
            {
                var collection = MongoDataContext.Database.GetCollection<TEntity>(_MongoAttribute.CollectionName);
                returnValue = (await collection.FindAsync(whereClause, MongoDataContext?.ClientSessionHandle)).Current.AsEnumerable();
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

            var collection = MongoDataContext.Database.GetCollection<TEntity>(_MongoAttribute.CollectionName);
            if (collection is null)
            {
                throw new MongoCollectionDoesNotExistException(_MongoAttribute.CollectionName, MongoDataContext.Database.DatabaseNamespace.DatabaseName);
            }

        }
    }
}
