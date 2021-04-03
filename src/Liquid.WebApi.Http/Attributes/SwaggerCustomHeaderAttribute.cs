using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Http.Attributes
{
    /// <summary>
    /// Swagger header base attribute class. Used to populate custom headers in Swagger.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    [ExcludeFromCodeCoverage]
    public class SwaggerCustomHeaderAttribute : SwaggerBaseHeaderAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerCustomHeaderAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="required">if set to <c>true</c> [required].</param>
        /// <param name="description">The description.</param>
        public SwaggerCustomHeaderAttribute(string name, bool required = false, string description = "") : base(name, required, description)
        {
        }
    }
}