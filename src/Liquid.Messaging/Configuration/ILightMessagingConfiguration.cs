using Liquid.Core.Configuration;

namespace Liquid.Messaging.Configuration
{
    /// <summary>
    /// Interface to implement the GetSettings customized to Messaging configuration.
    /// </summary>
    /// <typeparam name="TSettings">The type of the settings.</typeparam>
    /// <seealso cref="ILightConfiguration{TSettings}" />
    public interface ILightMessagingConfiguration<out TSettings> : ILightConfiguration<TSettings>
    {
        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <param name="configurationSection">The configuration section.</param>
        /// <returns></returns>
        public TSettings GetSettings(string configurationSection);
    }
}