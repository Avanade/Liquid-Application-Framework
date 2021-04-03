using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Http.Attributes
{
    /// <summary>
    /// Swagger authorization header attribute class.
    /// </summary>
    /// <seealso cref="SwaggerCustomHeaderAttribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    [ExcludeFromCodeCoverage]
    public class SwaggerAuthorizationHeaderAttribute : SwaggerBaseHeaderAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerAuthorizationHeaderAttribute"/> class.
        /// </summary>
        public SwaggerAuthorizationHeaderAttribute() : base("Authorization", true, "Example: bearer {your token}")
        {
        }
    }
}