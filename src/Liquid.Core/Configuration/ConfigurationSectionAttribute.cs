using System;

namespace Liquid.Core.Configuration
{
    /// <summary>
    /// Defines which configuration section, the custom configuration will read from json file.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigurationSectionAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the section.
        /// </summary>
        /// <value>
        /// The name of the section.
        /// </value>
        public string SectionName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSectionAttribute"/> class.
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        public ConfigurationSectionAttribute(string sectionName)
        {
            SectionName = sectionName;
        }  
    }
}