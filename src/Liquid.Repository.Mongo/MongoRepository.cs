using Liquid.Repository.Mongo.Configuration;
using Liquid.Repository.Mongo.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Liquid.Repository.Mongo
{
    /// <summary>
    /// Mongo database repository class. Implements the <seealso cref="ILiquidRepository{TEntity, TIdentifier}"/> interface to provide 
    /// the repository pattern access to a Mongo Db document. Also provides a Mongo data context to extend Mongo client resources.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TIdentifier">The type of the identifier.</typeparam>
    /// <seealso cref="ILiquidRepository{TEntity, TIdentifier}" />
    public class MongoRepository<TEntity, TIdentifier> : ILiquidRepository<TEntity, TIdentifier> where TEntity : LiquidEntity<TIdentifier>, new()
    {
        private readonly MongoEntityOptions _settings;

        ///<inheritdoc/>
        public IMongoDataContext<TEntity> MongoDataContext { get; }

        ///<inheritdoc/>
        public ILiquidDataContext DataContext => MongoDataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoRepository{TEntity, TIdentifier}" /> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        /// <exception cref="System.ArgumentNullException">
        /// telemetryFactory
        /// or
        /// dataContext
        /// </exception>
        public MongoRepository(IMongoDataContext<TEntity> dataContext)
        {
            MongoDataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            
            _settings = dataContext.Settings;

            MongoDataContext.SetDatabase(_settings.DatabaseName);            
        }

        ///<inheritdoc/>
        public async Task AddAsync(TEntity entity)
        {

            var collection = MongoDataContext.Database.GetCollection<TEntity>(_settings.CollectionName);
            await collection.InsertOneAsync(entity, MongoDataContext?.ClientSessionHandle);

        }

        ///<inheritdoc/>
        public async Task<IEnumerable<TEntity>> FindAllAsync()
        {
            var collection = MongoDataContext.Database.GetCollection<TEntity>(_settings.CollectionName);

            var response = await collection.FindAsync(new BsonDocument(), MongoDataContext?.ClientSessionHandle);
            var returnValue = response.ToEnumerable();

            return returnValue;
        }

        ///<inheritdoc/>
        public async Task<TEntity> FindByIdAsync(TIdentifier id)
        {
            var collection = MongoDataContext.Database.GetCollection<TEntity>(_settings.CollectionName);
            var result = await collection.FindAsync(e => e.Id.Equals(id), MongoDataContext?.ClientSessionHandle);
            var returnValue = result.SingleOrDefault();

            return returnValue;
        }

        ///<inheritdoc/>
        public async Task RemoveByIdAsync(TIdentifier id)
        {
            var collection = MongoDataContext.Database.GetCollection<TEntity>(_settings.CollectionName);
            await collection.DeleteOneAsync(e => e.Id.Equals(id), MongoDataContext?.ClientSessionHandle);
        }

        ///<inheritdoc/>
        public async Task UpdateAsync(TEntity entity)
        {

            var collection = MongoDataContext.Database.GetCollection<TEntity>(_settings.CollectionName);
            await collection.ReplaceOneAsync(x => x.Id.Equals(entity.Id), entity, new ReplaceOptions { IsUpsert = true }, MongoDataContext?.ClientSessionHandle);

        }

        ///<inheritdoc/>
        public async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> whereClause)
        {
            var collection = MongoDataContext.Database.GetCollection<TEntity>(_settings.CollectionName);

            var returnValue = (await collection.FindAsync(whereClause, MongoDataContext?.ClientSessionHandle)).ToEnumerable();

            return returnValue;
        }        
    }
}
