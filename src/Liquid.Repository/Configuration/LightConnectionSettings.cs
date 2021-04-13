using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Liguid.Repository.Configuration
{
    /// <summary>
    /// Light Configuration Settings custom class.
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class LightConnectionSettings
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the database connection string.
        /// </summary>
        /// <value>
        /// The database connection string.
        /// </value>
        [JsonProperty(PropertyName = "connectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        /// <value>
        /// The name of the database.
        /// </value>
        [JsonProperty(PropertyName = "databaseName")]
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        [JsonProperty(PropertyName = "parameters")]
        public List<DatabaseParameter> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the command timeout in seconds.
        /// </summary>
        /// <value>
        /// The command timeout.
        /// </value>
        [JsonProperty(PropertyName = "commandTimeout")]
        public int? CommandTimeout { get; set; }
    }

    /// <summary>
    /// Service parameter setting class.
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class DatabaseParameter
    {
        /// <summary>
        /// Gets or sets the parameter key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the parameter value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [JsonProperty("value")]
        public object Value { get; set; }
    }

    /// <summary>
    /// Connection setting extensions class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ConnectionSettingExtension
    {
        /// <summary>
        /// Gets the database connection setting by id.
        /// </summary>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static LightConnectionSettings GetConnectionSetting(this List<LightConnectionSettings> connectionSettings, string id)
        {
            var setting = connectionSettings.FirstOrDefault(cs => string.Equals(cs.Id, id, StringComparison.CurrentCultureIgnoreCase));
            return setting;
        }

        /// <summary>
        /// Gets the database parameter value.
        /// </summary>
        /// <typeparam name="TObject">The type of the parameter value.</typeparam>
        /// <param name="parameters">The parameter list.</param>
        /// <param name="key">The parameter key.</param>
        /// <param name="required">if set to <c>true</c> [required].</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// the parameter value in TObject type.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// key
        /// or
        /// key
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">key
        /// or
        /// key</exception>
        public static TObject GetDatabaseParameter<TObject>(this List<DatabaseParameter> parameters, string key, bool required = false, TObject defaultValue = default)
        {
            var parameter = parameters.FirstOrDefault(p => key.Equals(p.Key, StringComparison.InvariantCultureIgnoreCase));
            if (parameter != null)
            {
                var objectValue = parameter.Value;
                switch (typeof(TObject).Name.ToLowerInvariant())
                {
                    case "string":
                    case "int32":
                    case "int64":
                    case "boolean":
                        return (TObject)Convert.ChangeType(objectValue, typeof(TObject));
                    case "timespan":
                        TimeSpan.TryParse((string)objectValue, out var returnValue);
                        return (TObject)Convert.ChangeType(returnValue, typeof(TObject));
                }
                throw new ArgumentOutOfRangeException(nameof(key), $"{key} key type error, expected {typeof(TObject)}. Sent {parameter.Value.GetType()}");
            }
            if (required) throw new ArgumentOutOfRangeException(nameof(key), $"{key} key Not found in parameters collection.");

            return defaultValue;
        }
    }
}
