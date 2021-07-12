using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Repository
{
    /// <summary>
    /// Represents the repository entity
    /// </summary>
    /// <typeparam name="TIdentifier">The type of the identifier.</typeparam>
    public interface ILiquidEntity<TIdentifier>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        TIdentifier Id { get; set; }
    }
}
