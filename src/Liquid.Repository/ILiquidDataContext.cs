using System;
using System.Threading.Tasks;

namespace Liquid.Repository
{
    /// <summary>
    /// Represents the database context
    /// </summary>
    public interface ILiquidDataContext : IDisposable
    {
        /// <summary>
        /// Gets the identifier of data context.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        string Id { get; }

        /// <summary>
        /// Starts the transaction of all data contexts in repositories inside UnitOfWork.
        /// </summary>
        /// <returns></returns>
        Task StartTransactionAsync();

        /// <summary>
        /// Commits all commands added to the database context.
        /// </summary>
        /// <returns></returns>
        Task CommitAsync();

        /// <summary>
        /// Rollbacks the transactions.
        /// </summary>
        /// <returns></returns>
        Task RollbackTransactionAsync();
    }
}
