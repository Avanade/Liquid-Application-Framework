using System;
using System.Globalization;

namespace Liquid.Core.Localization
{
    /// <summary>
    /// Resource catalog interface.
    /// </summary>
    [Obsolete("This class will be removed or refactored in the next release.")]
    public interface ILocalization
    {
        /// <summary>
        /// Gets the specified string according to culture.
        /// </summary>
        /// <param name="key">The resource key.</param>
        /// <returns>
        /// The string associated with resource key.
        /// </returns>
        [Obsolete("This method will be removed or refactored in the next release.")]
        string Get(string key);

        /// <summary>
        /// Gets the specified string according to culture.
        /// </summary>
        /// <param name="key">The resource key.</param>
        /// <param name="channel">The channel.</param>
        /// <returns>
        /// The string associated with resource key.
        /// </returns>
        [Obsolete("This method will be removed or refactored in the next release.")]
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
        [Obsolete("This method will be removed or refactored in the next release.")]
        string Get(string key, CultureInfo culture, string channel = null);
    }
}