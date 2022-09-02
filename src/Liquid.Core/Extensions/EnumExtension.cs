using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Liquid.Core.Extensions
{
    /// <summary>
    /// Enum Extensions Class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class EnumExtension
    {
        /// <summary>
        /// Gets the description from enum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The enum description.</returns>
        public static string GetDescription(this Enum value)
        {
            var attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

        /// <summary>
        /// Gets the enum attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>the enum attribute.</returns>
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);

            return type.GetField(name)
                .GetCustomAttributes(false)
                .OfType<T>()
                .SingleOrDefault();
        }
    }
}
