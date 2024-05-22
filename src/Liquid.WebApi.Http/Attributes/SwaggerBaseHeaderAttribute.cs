using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Http.Attributes
{
    /// <summary>
    /// Swagger base parameter attribute class.
    /// </summary>
    /// <seealso cref="Attribute" />
    /// <remarks>
    /// Initializes a new instance of the <see cref="SwaggerCustomHeaderAttribute" /> class.
    /// </remarks>
    /// <param name="name">The name.</param>
    /// <param name="required">if set to <c>true</c> [required].</param>
    /// <param name="description">The description.</param>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    [ExcludeFromCodeCoverage]
    public abstract class SwaggerBaseHeaderAttribute(string name, bool required = false, string description = "") : Attribute
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; } = name;

        /// <summary>
        /// Gets a value indicating whether this <see cref="SwaggerCustomHeaderAttribute"/> is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        public bool Required { get; } = required;

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; } = description;
    }
}