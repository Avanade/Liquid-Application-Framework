using Liquid.Core.Attributes;
using Liquid.Repository.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Liquid.Repository.Mongo.Configuration
{
    /// <summary>
    /// MongoDB configurations List
    /// </summary>
    [LiquidSectionName("liquid:databases:mongo")]
    public class MongoSettings
    {
        /// <summary>
        /// List of mongo settings.
        /// </summary>
        public IList<DatabaseSettings> DbSettings { get; set; } 

        /// <summary>
        /// Retrieve settings from a given database.
        /// </summary>
        /// <param name="databaseName">database name to have its settings retrieved.</param>
        /// <returns><see cref="DatabaseSettings"/></returns>
        public DatabaseSettings GetDbSettings(string databaseName)
        {
            return DbSettings.FirstOrDefault(x => x.DatabaseName == databaseName);
        }
    }


}
