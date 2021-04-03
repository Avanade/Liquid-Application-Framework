using Liquid.Services.Configuration;

namespace Liquid.Services
{
    /// <summary>
    /// Service client interface.
    /// </summary>
    public interface ILightService
    {
        /// <summary>
        /// Gets the service identifier.
        /// </summary>
        /// <value>
        /// The service identifier.
        /// </value>
        string ServiceId { get; }

        /// <summary>
        /// Gets the service settings.
        /// </summary>
        /// <value>
        /// The service settings.
        /// </value>
        LightServiceSetting ServiceSettings { get; }
    }
}