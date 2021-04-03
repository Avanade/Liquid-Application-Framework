using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Http.Configuration
{
    /// <summary>
    /// Swagger Configuration Settings Class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SwaggerSettings
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        [JsonProperty("host")]
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets schemes.
        /// </summary>
        /// <value>
        /// Schemes.
        /// </value>
        [JsonProperty("schemes")]
        public string[] Schemes { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [JsonProperty("version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the swagger endpoint.
        /// </summary>
        /// <value>
        /// The swagger endpoint.
        /// </value>
        [JsonProperty("endpoint")]
        public SwaggerEndpoint SwaggerEndpoint { get; set; }
    }

    /// <summary>
    /// Swagger Endpoint Class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SwaggerEndpoint
    {
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}