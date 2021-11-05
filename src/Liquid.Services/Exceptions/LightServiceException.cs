using Liquid.Core.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Services.Exceptions
{
    /// <summary>
    /// Occurs when a generic service exception has occured.
    /// </summary>
    /// <seealso cref="Liquid.Core.Exceptions.LiquidException" />
    [ExcludeFromCodeCoverage]
    public class LightServiceException : LiquidException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LightServiceException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public LightServiceException(string message) : base(message)
        { }
    }
}