using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Exceptions;

namespace Liquid.Services.Exceptions
{
    /// <summary>
    /// Occurs when a generic service exception has occured.
    /// </summary>
    /// <seealso cref="Liquid.Core.Exceptions.LightException" />
    [ExcludeFromCodeCoverage]
    public class LightServiceException : LightException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LightServiceException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public LightServiceException(string message) : base(message)
        { }
    }
}