using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Http.Attributes
{
    /// <summary>
    /// Swagger header base attribute class. Used to populate custom headers in Swagger.
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
    public class SwaggerCustomHeaderAttribute(string name, bool required = false, string description = "") : SwaggerBaseHeaderAttribute(name, required, description)
    {
    }
}