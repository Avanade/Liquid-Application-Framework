using System;
using Liquid.Messaging.Configuration;
using Liquid.Messaging.Extensions;

namespace Liquid.Messaging.Kafka.Extensions
{
    /// <summary>
    /// Connection Extensions
    /// </summary>
    internal static class ConnectionExtensions
    {
        /// <summary>
        /// Gets the project identifier.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public static int? GetTimeout(this MessagingSettings settings)
        {
            try
            {
                return settings?.Parameters?.GetCustomParameter<int>("Timeout");
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the socket keep alive.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public static bool? GetSocketKeepAlive(this MessagingSettings settings)
        {
            try
            {
                return settings?.Parameters?.GetCustomParameter<bool>("SocketKeepalive");
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }
    }
}