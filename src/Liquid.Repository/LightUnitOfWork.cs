using Liquid.Repository.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Liquid.Repository
{
    /// <summary>
    /// Controls transactions in all data contexts
    /// </summary>
    /// <seealso cref="Liquid.Repository.ILightUnitOfWork" />
    public class LightUnitOfWork : ILightUnitOfWork
    {
        private readonly List<ILightDataContext> _datacontexts = new List<ILightDataContext>();
        private readonly IServiceProvider _serviceProvider;
        private bool _transactionStarted;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightUnitOfWork"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <exception cref="ArgumentNullException">serviceProvider</exception>
        public LightUnitOfWork(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Gets the repository from Service Provider and adds to UnitOfWork.
        /// </summary>
        /// <typeparam name="TRepository">The type of the repository.</typeparam>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TIdentifier">The type of the identifier.</typeparam>
        /// <returns></returns>
        public TRepository GetRepository<TRepository, TEntity, TIdentifier>()
            where TRepository : ILightRepository<TEntity, TIdentifier>
            where TEntity : RepositoryEntity<TIdentifier>
        {
            var repository = _serviceProvider.GetService<TRepository>();

            if (!_datacontexts.Any(dataContext => dataContext.Id == repository.DataContext.Id))
            {
                _datacontexts.Add(repository.DataContext);
            }

            return repository;
        }

        /// <summary>
        /// Starts the transaction of all data contexts in repositories inside UnitOfWork.
        /// </summary>
        /// <exception cref="Liquid.Repository.Exceptions.UnitofWorkTransactionWithoutRepositoryException"></exception>
        public async Task StartTransactionAsync()
        {
            if (!_datacontexts.Any()) throw new UnitofWorkTransactionWithoutRepositoryException();
            if (!_transactionStarted)
            {
                foreach (var datacontext in _datacontexts)
                {
                    await datacontext.StartTransactionAsync();
                }
            }
            _transactionStarted = true;
        }

        /// <summary>
        /// Commits all commands added to the database context.
        /// </summary>
        /// <exception cref="Liquid.Repository.Exceptions.UnitOfWorkTransactionNotStartedException"></exception>
        public async Task CommitAsync()
        {
            if (!_transactionStarted) throw new UnitOfWorkTransactionNotStartedException();
            foreach (var datacontext in _datacontexts)
            {
                await datacontext.CommitAsync();
            }
            _transactionStarted = false;
            _datacontexts.Clear();
        }

        /// <summary>
        /// Rollbacks the transactions.
        /// </summary>
        /// <exception cref="Liquid.Repository.Exceptions.UnitOfWorkTransactionNotStartedException"></exception>
        public async Task RollbackTransactionAsync()
        {
            if (!_transactionStarted) throw new UnitOfWorkTransactionNotStartedException();
            foreach (var datacontext in _datacontexts)
            {
                await datacontext.RollbackTransactionAsync();
            }
            _transactionStarted = false;
            _datacontexts.Clear();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {

        }
    }
}
