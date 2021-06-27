using System;

namespace Liquid.Core.Attributes
{
    /// <summary>
    /// Defines which configuration section, the custom configuration will read from json file.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class LiquidSectionNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the section.
        /// </summary>
        /// <value>
        /// The name of the section.
        /// </value>
        public string SectionName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LiquidSectionNameAttribute"/> class.
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        public LiquidSectionNameAttribute(string sectionName)
        {
            SectionName = sectionName;
        }
    }
}