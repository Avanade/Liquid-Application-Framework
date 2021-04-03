using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Http.Attributes
{
    /// <summary>
    /// Swagger base parameter attribute class.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    [ExcludeFromCodeCoverage]
    public abstract class SwaggerBaseHeaderAttribute : Attribute
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="SwaggerCustomHeaderAttribute"/> is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        public bool Required { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerCustomHeaderAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="required">if set to <c>true</c> [required].</param>
        /// <param name="description">The description.</param>
        protected SwaggerBaseHeaderAttribute(string name, bool required = false, string description = "")
        {
            Name = name;
            Required = required;
            Description = description;
        }
    }
}