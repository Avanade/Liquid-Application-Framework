using Liquid.Core.Entities;
using Liquid.Core.Interfaces;
using Simple.OData.Client;
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
        private string _token;

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

        /// <summary>
        /// Set the token to perform operations.
        /// </summary>
        /// <param name="token">Token to be set.</param>
        public void SetToken(string token)
        {
            _token = token;
        }

        ///<inheritdoc/>
        public async Task AddAsync(TEntity entity)
        {
            if (string.IsNullOrEmpty(_token))
            {
                throw new InvalidOperationException("Token is required to perform this operation.");
            }

            var client = _clientFactory.CreateODataClientAsync(_entityName, _token);

            await client.For<TEntity>().Set(entity).InsertEntryAsync();
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<TEntity>> FindAllAsync()
        {
            if (string.IsNullOrEmpty(_token))
            {
                throw new InvalidOperationException("Token is required to perform this operation.");
            }

            var client = _clientFactory.CreateODataClientAsync(_entityName, _token);

            return await client.For<TEntity>().FindEntriesAsync();
        }

        ///<inheritdoc/>
        public async Task<TEntity> FindByIdAsync(TIdentifier id)
        {
            if (string.IsNullOrEmpty(_token))
            {
                throw new InvalidOperationException("Token is required to perform this operation.");
            }
            var client = _clientFactory.CreateODataClientAsync(_entityName, _token);

            return await client.For<TEntity>().Key(id).FindEntryAsync();
        }

        ///<inheritdoc/>
        public async Task RemoveByIdAsync(TIdentifier id)
        {
            if (string.IsNullOrEmpty(_token))
            {
                throw new InvalidOperationException("Token is required to perform this operation.");
            }

            var client = _clientFactory.CreateODataClientAsync(_entityName, _token);

            await client.For<TEntity>().Key(id).DeleteEntryAsync();
        }

        ///<inheritdoc/>
        public async Task UpdateAsync(TEntity entity)
        {
            if (string.IsNullOrEmpty(_token))
            {
                throw new InvalidOperationException("Token is required to perform this operation.");
            }

            var client = _clientFactory.CreateODataClientAsync(_entityName, _token);

            await client.For<TEntity>().Set(entity).UpdateEntryAsync();
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> whereClause)
        {
            if (string.IsNullOrEmpty(_token))
            {
                throw new InvalidOperationException("Token is required to perform this operation.");
            }

            var client = _clientFactory.CreateODataClientAsync(_entityName, _token);

            return await client.For<TEntity>().Filter(whereClause).FindEntriesAsync();
        }
    }
}
