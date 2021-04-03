using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Http.Attributes
{
    /// <summary>
    /// Culture header attribute class.
    /// </summary>
    /// <seealso cref="SwaggerCustomHeaderAttribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    [ExcludeFromCodeCoverage]
    public class SwaggerCultureHeaderAttribute : SwaggerBaseHeaderAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerCultureHeaderAttribute"/> class.
        /// </summary>
        public SwaggerCultureHeaderAttribute() : base("Culture", false, "Example: pt-BR, en-US")
        {
        }
    }
}