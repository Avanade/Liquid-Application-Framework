using Liquid.Core.Entities;
using Liquid.Core.Interfaces;
using System.Linq.Expressions;

namespace Liquid.Repository.OData
{
    ///<inheritdoc/>
    public class ODataRepository<TEntity, TIdentifier> : ILiquidRepository<TEntity, TIdentifier> where TEntity : LiquidEntity<TIdentifier>, new()
    {
        ///<inheritdoc/>
        public ILiquidDataContext DataContext => throw new NotImplementedException();

        private readonly IODataClientFactory _clientFactory;
        private readonly string _entityName;

        /// <summary>
        /// Initialize a new instance of <see cref="ODataRepository{TEntity, TIdentifier}"/>
        /// </summary>
        /// <param name="clientFactory"> Factory to create OData client.</param>
        /// <param name="entityName"> Name of the entity to be used in the repository.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ODataRepository(IODataClientFactory clientFactory, string entityName)
        {
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _entityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
        }


        ///<inheritdoc/>
        public async Task AddAsync(TEntity entity)
        {   
            var client = _clientFactory.CreateODataClientAsync(_entityName);

            await client.For<TEntity>().Set(entity).InsertEntryAsync();
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<TEntity>> FindAllAsync()
        {
            var client = _clientFactory.CreateODataClientAsync(_entityName);

            return await client.For<TEntity>().FindEntriesAsync();
        }

        ///<inheritdoc/>
        public async Task<TEntity> FindByIdAsync(TIdentifier id)
        {
            var client = _clientFactory.CreateODataClientAsync(_entityName);

            return await client.For<TEntity>().Key(id).FindEntryAsync();
        }

        ///<inheritdoc/>
        public async Task RemoveByIdAsync(TIdentifier id)
        {
            var client = _clientFactory.CreateODataClientAsync(_entityName);

            await client.For<TEntity>().Key(id).DeleteEntryAsync();
        }

        ///<inheritdoc/>
        public async Task UpdateAsync(TEntity entity)
        {
            var client = _clientFactory.CreateODataClientAsync(_entityName);

            await client.For<TEntity>().Set(entity).UpdateEntryAsync();
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> whereClause)
        {
            var client = _clientFactory.CreateODataClientAsync(_entityName);

            return await client.For<TEntity>().Filter(whereClause).FindEntriesAsync();
        }
    }
}
