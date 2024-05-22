using System;

namespace Liquid.Core.Attributes
{
    /// <summary>
    /// Defines which configuration section, the custom configuration will read from json file.
    /// </summary>
    /// <seealso cref="Attribute" />
    /// <remarks>
    /// Initializes a new instance of the <see cref="LiquidSectionNameAttribute"/> class.
    /// </remarks>
    /// <param name="sectionName">Name of the section.</param>
    [AttributeUsage(AttributeTargets.Class)]
    public class LiquidSectionNameAttribute(string sectionName) : Attribute
    {
        /// <summary>
        /// Gets the name of the section.
        /// </summary>
        /// <value>
        /// The name of the section.
        /// </value>
        public string SectionName { get; } = sectionName;
    }
}