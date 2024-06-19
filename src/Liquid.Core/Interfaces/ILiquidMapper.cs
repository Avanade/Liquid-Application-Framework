using System.Threading.Tasks;

namespace Liquid.Core.Interfaces
{
    /// <summary>
    /// Defines object that map data between two instance types.
    /// </summary>
    /// <typeparam name="TFrom">type of data source object.</typeparam>
    /// <typeparam name="TTo">results object type.</typeparam>
    public interface ILiquidMapper<TFrom, TTo>
    {
        /// <summary>
        /// Create a new instance of <see cref="TTo"/>
        /// with values obtained from <see cref="TFrom"/>.
        /// </summary>
        /// <param name="dataObject">data source object instance.</param>
        Task<TTo> Map(TFrom dataObject);
    }
}
