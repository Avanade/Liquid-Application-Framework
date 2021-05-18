using Liquid.Core.Configuration;


namespace Liquid.Repository.Configuration
{
    /// <summary>
    /// Interface to implement the GetSettings customized to database configuration.
    /// </summary>
    /// <typeparam name="TSettings">The type of the settings.</typeparam>
    /// <seealso cref="ILightConfiguration{TSettings}" />
    public interface ILightDatabaseConfiguration<out TSettings> : ILightConfiguration<TSettings>
    {
        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <param name="configurationSection">The configuration section.</param>
        /// <returns></returns>
        public TSettings GetSettings(string configurationSection);
    }
}
