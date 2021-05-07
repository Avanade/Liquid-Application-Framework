using Amazon;
using Liquid.Core.Configuration;
using Liquid.Messaging.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;

namespace Liquid.Messaging.Aws.Configuration
{
    /// <summary>
    /// Aws Messaging Configuration Class.
    /// </summary>
    /// <seealso cref="LightConfiguration" />
    /// <seealso>
    /// <cref>
    ///     Core.Configuration.ILightConfiguration{Messaging.Configuration.MessagingSettings}
    /// </cref>
    /// </seealso>
    public class AwsMessagingConfiguration : LightConfiguration, ILightMessagingConfiguration<AwsMessagingSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwsMessagingConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public AwsMessagingConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        /// <inheritdoc/>
        public AwsMessagingSettings Settings => throw new NotImplementedException(); //Not Used

        /// <inheritdoc/>
        public AwsMessagingSettings GetSettings(string configurationSection)
        {
            return GetConfigurationSection<AwsMessagingSettings>($"liquid:messaging:aws:{configurationSection}");
        }
    }

    /// <summary>
    /// Aws Messaging Settings
    /// </summary>
    public class AwsMessagingSettings
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the access key.
        /// </summary>
        /// <value>
        /// The access key.
        /// </value>
        [JsonProperty("accessKey")]
        public string AccessKey { get; set; }

        /// <summary>
        /// Gets or sets the secret key.
        /// </summary>
        /// <value>
        /// The secret key.
        /// </value>
        [JsonProperty("secretKey")]
        public string SecretKey { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        [JsonProperty("region")]
        public string Region { get; set; }
    }

    /// <summary>
    /// AWS Messaging Settings Extensions Class.
    /// </summary>
    internal static class AwsMessagingSettingsExtensions
    {
        /// <summary>
        /// Gets the region.
        /// </summary>
        /// <param name="messagingSettings">The messaging settings.</param>
        /// <returns></returns>
        public static RegionEndpoint GetRegion(this AwsMessagingSettings messagingSettings)
        {
            return messagingSettings.Region != null ? RegionEndpoint.GetBySystemName(messagingSettings.Region) : RegionEndpoint.USEast1;
        }
    }
}
