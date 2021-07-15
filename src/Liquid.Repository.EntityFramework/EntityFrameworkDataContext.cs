using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Liquid.Repository.EntityFramework
{
    /// <summary>
    /// Implements the EntityFramework data context for repositories.
    /// </summary>
    /// <seealso cref="Liquid.Repository.EntityFramework.EntityFrameworkDataContext{TContext}" />
    /// <typeparam name="TContext">The type of the <see cref="DbContext"/>.</typeparam>
    public class EntityFrameworkDataContext<TContext> : IEntityFrameworkDataContext<TContext> where TContext : DbContext
    {
        private bool _disposed = false;
        private readonly TContext _databaseContext;

        /// <summary>
        /// Gets the identifier of data context.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; }

        ///<inheritdoc/>
        public TContext DbClient => _databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkDataContext{TContext}" /> class.
        /// </summary>
        /// <param name="dbContext">Data base context object <see cref="DbContext"/>.</param>
        public EntityFrameworkDataContext(TContext dbContext)
        {
            _databaseContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        ///<inheritdoc/>
        public async Task StartTransactionAsync()
        {

            await _databaseContext.Database.BeginTransactionAsync();
        }

        ///<inheritdoc/>
        public async Task CommitAsync()
        {
            await _databaseContext.Database.CommitTransactionAsync();
        }

        ///<inheritdoc/>
        public async Task RollbackTransactionAsync()
        {
            await _databaseContext.Database.RollbackTransactionAsync();
        }

        ///<inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the allocated resources <see cref="DbContext"/> for this context.
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
                _databaseContext.Dispose();
            }

            _disposed = true;
        }

    }
}
