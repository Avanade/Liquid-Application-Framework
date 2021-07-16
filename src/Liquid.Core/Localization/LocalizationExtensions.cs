using Liquid.Core.Localization.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Liquid.Core.Localization
{
    /// <summary>
    /// Resource extensions class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class LocalizationExtensions
    {
        /// <summary>
        /// Gets the resource item from collection.
        /// </summary>
        /// <param name="resources">The resources.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        internal static LocalizationItem GetResourceItem(this IEnumerable<LocalizationItem> resources, string key)
        {
            var result = resources.FirstOrDefault(r => r.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
            return result;
        }

        /// <summary>
        /// Gets the resource value.
        /// </summary>
        /// <param name="resourceItem">The resource item.</param>
        /// <param name="channel">The channel.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">cultureCode</exception>
        internal static LocalizationValue GetResourceValue(this LocalizationItem resourceItem, string channel = null)
        {
            var values = resourceItem.Values;
            if (channel == null) { return values.FirstOrDefault(); }
            var resourceValues = values as LocalizationValue[] ?? values.ToArray();
            var returnValue = resourceValues.FirstOrDefault(v => v.ChannelsList.Contains(channel.ToLower()));
            return returnValue ?? resourceValues.FirstOrDefault();
        }

        /// <summary>
        /// Adds the localization using the default filename localization.json
        /// </summary>
        /// <param name="services">The services.</param>
        [Obsolete("This method will be removed or refactored in the next release.")]
        public static void AddLocalizationService(this IServiceCollection services)
        {
            services.AddSingleton<ILocalization, JsonFileLocalization>();
        }
    }
}