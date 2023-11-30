using Liquid.Core.Attributes;
using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Http.Settings
{
    /// <summary>
    /// Swagger Configuration Settings Class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [LiquidSectionName("liquid:swagger")]
    public class SwaggerSettings
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        [JsonPropertyName("host")]
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets schemes.
        /// </summary>
        /// <value>
        /// Schemes.
        /// </value>
        [JsonPropertyName("schemes")]
        public string[] Schemes { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [JsonPropertyName("version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the swagger endpoint.
        /// </summary>
        /// <value>
        /// The swagger endpoint.
        /// </value>
        [JsonPropertyName("endpoint")]
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
        [JsonPropertyName("url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}