using System.Diagnostics.CodeAnalysis;

namespace Liquid.Repository.Configuration
{
    /// <summary>
    /// Database configuration properties.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DatabaseSettings
    {
        /// <summary>
        /// Gets or sets the database connection string.
        /// </summary>
        /// <value>
        /// The database connection string.
        /// </value>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        /// <value>
        /// The name of the database.
        /// </value>
        public string DatabaseName { get; set; }
    }
}
