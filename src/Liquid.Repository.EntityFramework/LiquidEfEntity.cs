using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Liquid.Repository.EntityFramework
{
    /// <summary>
    /// Base implementation of <see cref="ILiquidEntity{TIdentifier}"/> for Entity Framework adapter.
    /// </summary>
    /// <typeparam name="TIdentifier">The type of the identifier.</typeparam>
    [ExcludeFromCodeCoverage]
    public class LiquidEfEntity<TIdentifier> : ILiquidEntity<TIdentifier>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public virtual TIdentifier Id { get; set; }
    }
}
