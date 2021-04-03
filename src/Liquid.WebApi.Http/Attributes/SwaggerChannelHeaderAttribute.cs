using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Http.Attributes
{
    /// <summary>
    /// Adds a "Channel" header to Swagger methods.
    /// </summary>
    /// <seealso cref="SwaggerCustomHeaderAttribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    [ExcludeFromCodeCoverage]
    public class SwaggerChannelHeaderAttribute : SwaggerBaseHeaderAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerChannelHeaderAttribute" /> class.
        /// </summary>
        public SwaggerChannelHeaderAttribute() : base("Channel", false, "Example: ios, android, web. default value: web")
        {
        }
    }
}
