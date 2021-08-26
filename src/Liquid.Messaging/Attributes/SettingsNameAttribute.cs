using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Messaging.Attributes
{
    /// <summary>
    /// Messaging configuration section name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [ExcludeFromCodeCoverage]
    public class SettingsNameAttribute : Attribute
    {
        /// <summary>
        /// Name of section that contains a specific configuration of queue/topic.
        /// </summary>
        public string SettingsName { get; set; }

        /// <summary>
        /// Initialize a new instance of <see cref="SettingsNameAttribute"/>.
        /// </summary>
        /// <param name="settingsName">Name of section that contains a specific configuration of queue/topic.</param>
        public SettingsNameAttribute(string settingsName)
        {
            SettingsName = settingsName ?? throw new ArgumentNullException(nameof(settingsName));
        }
    }
}
