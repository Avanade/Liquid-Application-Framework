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
    /// <seealso cref="Liquid.Repository.EntityFramework.IEntityFrameworkDataContext" />
    public class EntityFrameworkDataContext : IEntityFrameworkDataContext, IDisposable
    {
        private readonly DbContext _databaseContext;
        private readonly ILightTelemetryFactory _telemetryFactory;

        /// <summary>
        /// Gets the identifier of data context.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; }

        ///<inheritdoc/>
        public DbContext DbClient => _databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkDataContext" /> class.
        /// </summary>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="clientFactory">EntityFramework Client generator.</param>
        /// <param name="connectionId">Database connection configuration Id.</param>
        public EntityFrameworkDataContext(ILightTelemetryFactory telemetryFactory
            , IEntityFrameworkClientFactory clientFactory, string connectionId)
        {
            _telemetryFactory = telemetryFactory ?? throw new ArgumentNullException(nameof(telemetryFactory));
            if (clientFactory is null) throw new ArgumentNullException(nameof(clientFactory));
            if (connectionId is null) throw new ArgumentNullException(nameof(connectionId));

            _databaseContext = clientFactory.GetClient(connectionId);
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
