using System.Globalization;

namespace Liquid.Core.Localization
{
    /// <summary>
    /// Resource catalog interface.
    /// </summary>
    public interface ILocalization 
    {
        /// <summary>
        /// Gets the specified string according to culture.
        /// </summary>
        /// <param name="key">The resource key.</param>
        /// <returns>
        /// The string associated with resource key.
        /// </returns>
        string Get(string key);

        /// <summary>
        /// Gets the specified string according to culture.
        /// </summary>
        /// <param name="key">The resource key.</param>
        /// <param name="channel">The channel.</param>
        /// <returns>
        /// The string associated with resource key.
        /// </returns>
        string Get(string key, string channel);

        /// <summary>
        /// Gets the specified string according to culture.
        /// </summary>
        /// <param name="key">The resource key.</param>
        /// <param name="culture">The culture.</param>
        /// <param name="channel">The channel.</param>
        /// <returns>
        /// The string associated with resource key.
        /// </returns>
        string Get(string key, CultureInfo culture, string channel = null);
    }
}