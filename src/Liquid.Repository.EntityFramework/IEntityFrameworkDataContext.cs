using Microsoft.EntityFrameworkCore;

namespace Liquid.Repository.EntityFramework
{
    /// <summary>
    /// EntityFramework database context interface.
    /// </summary>
    /// <seealso cref="Liquid.Repository.ILightDataContext" />
    public interface IEntityFrameworkDataContext : ILightDataContext
    {
        /// <summary>
        /// Gets the Entity Framework client.
        /// </summary>
        /// <value>
        /// The Entity Framework client.
        /// </value>
        DbContext DbClient { get; }
    }
}