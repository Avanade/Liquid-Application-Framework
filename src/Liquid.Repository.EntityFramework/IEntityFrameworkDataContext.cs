using Microsoft.EntityFrameworkCore;

namespace Liquid.Repository.EntityFramework
{
    /// <summary>
    /// EntityFramework database context interface.
    /// </summary>
    /// <typeparam name="TContext">The type of the <see cref="DbContext"/>.</typeparam>
    /// <seealso cref="Liquid.Repository.ILiquidDataContext" />
    public interface IEntityFrameworkDataContext<out TContext> : ILiquidDataContext where TContext : DbContext
    {
        /// <summary>
        /// Gets the Entity Framework client.
        /// </summary>
        /// <value>
        /// The Entity Framework client.
        /// </value>
        TContext DbClient { get; }
    }
}