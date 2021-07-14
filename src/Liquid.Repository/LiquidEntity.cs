namespace Liquid.Repository
{
    /// <summary>
    /// Represents the repository entity
    /// </summary>
    /// <typeparam name="TIdentifier">The type of the identifier.</typeparam>
    public class LiquidEntity<TIdentifier>
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
