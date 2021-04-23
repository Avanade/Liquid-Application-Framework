using Microsoft.EntityFrameworkCore;

namespace Liquid.Repository.EntityFramework
{
    /// <summary>
    /// Provide client generator methods.
    /// </summary>
    public interface IEntityFrameworkClientFactory
    {
        /// <summary>
        /// Provide a new instance of <see cref="DbContext"/> with db conection started.
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        DbContext GetClient(string connectionId);
    }
}
