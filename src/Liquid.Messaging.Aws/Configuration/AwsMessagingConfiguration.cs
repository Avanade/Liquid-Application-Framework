using Amazon;
using Liquid.Core.Attributes;
using Newtonsoft.Json;

namespace Liquid.Messaging.Aws.Configuration
{

    /// <summary>
    /// Aws Messaging Settings
    /// </summary>
    [LiquidSectionName("liquid:messaging:aws:")]
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
