using Liquid.Messaging.Configuration;
using Liquid.Messaging.Extensions;

namespace Liquid.Messaging.Gcp.Extensions
{
    /// <summary>
    /// Connection Extensions
    /// </summary>
    internal static  class ConnectionExtensions
    {
        /// <summary>
        /// Gets the project identifier.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public static string GetProjectId(this MessagingSettings settings)
        {
            return settings?.Parameters?.GetCustomParameter<string>("ProjectId");
        }
    }
}