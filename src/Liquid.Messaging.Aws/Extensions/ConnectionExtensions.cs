using Amazon;
using Liquid.Messaging.Configuration;
using Liquid.Messaging.Extensions;

namespace Liquid.Messaging.Aws.Extensions
{
    /// <summary>
    /// Connection Extensions
    /// </summary>
    internal static  class ConnectionExtensions
    {
        /// <summary>
        /// Gets the aws access key.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public static string GetAwsAccessKey(this MessagingSettings settings)
        {
            return settings?.Parameters?.GetCustomParameter<string>("AwsAccessKey");
        }

        /// <summary>
        /// Gets the aws secret key.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public static string GetAwsSecretKey(this MessagingSettings settings)
        {
            return settings?.Parameters?.GetCustomParameter<string>("AwsSecretKey");
        }

        /// <summary>
        /// Gets the aws region.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public static RegionEndpoint GetAwsRegion(this MessagingSettings settings)
        {
            var regionSetting = settings?.Parameters?.GetCustomParameter<string>("AwsRegion");

            return regionSetting != null ? RegionEndpoint.GetBySystemName(regionSetting) : RegionEndpoint.USEast1;
        }
    }
}