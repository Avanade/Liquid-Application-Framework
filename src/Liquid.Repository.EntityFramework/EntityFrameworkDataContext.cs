using Liquid.Core.Telemetry;
using Liquid.Repository.Extensions;
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
    public class EntityFrameworkDataContext<TContext> : IEntityFrameworkDataContext<TContext>, IDisposable where TContext : DbContext
    {
        private readonly TContext _databaseContext;
        private readonly ILightTelemetryFactory _telemetryFactory;

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
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="dbContext">Data base context object <see cref="DbContext"/>.</param>
        public EntityFrameworkDataContext(ILightTelemetryFactory telemetryFactory
            , TContext dbContext)
        {
            _telemetryFactory = telemetryFactory ?? throw new ArgumentNullException(nameof(telemetryFactory));
            _databaseContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        ///<inheritdoc/>
        public async Task StartTransactionAsync()
        {
            await _telemetryFactory.ExecuteActionAsync("EntityFrameworkRepository_StartTransctionAsync", async () =>
            {
                await _databaseContext.Database.BeginTransactionAsync();
            });
        }

        ///<inheritdoc/>
        public async Task CommitAsync()
        {
            await _telemetryFactory.ExecuteActionAsync("EntityFrameworkRepository_CommitAsync", async () =>
            {
                await _databaseContext.Database.CommitTransactionAsync();
            });
        }

        ///<inheritdoc/>
        public async Task RollbackTransactionAsync()
        {
            await _telemetryFactory.ExecuteActionAsync("EntityFrameworkRepository_RollbackTransactionAsync", async () =>
            {
                await _databaseContext.Database.RollbackTransactionAsync();
            });
        }

        ///<inheritdoc/>
        public void Dispose()
        {
            _databaseContext.Dispose();
        }

    }
}
