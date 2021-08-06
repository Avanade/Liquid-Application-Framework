using Liquid.Core.Base;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Core.Exceptions
{
    /// <summary>
    /// Contains Exceptions Custom Codes to be processed and converted to custom error codes.
    /// </summary>
    /// <seealso cref="Liquid.Core.Base.Enumeration" />
    [ExcludeFromCodeCoverage]
    public class ExceptionCustomCodes : Enumeration
    {
        /// <summary>
        /// Indicates that the item is not found.
        /// </summary>
        public static readonly ExceptionCustomCodes NotFound = new ExceptionCustomCodes(404, "NotFound");

        /// <summary>
        /// Indicates that an internal error has occurred.
        /// </summary>
        public static readonly ExceptionCustomCodes InternalError = new ExceptionCustomCodes(500, "InternalError");

        /// <summary>
        /// Indicates that the request is bad formatted a validation error has occurred.
        /// </summary>
        public static readonly ExceptionCustomCodes BadRequest = new ExceptionCustomCodes(400, "BadRequest");

        /// <summary>
        /// Indicates a conflict.
        /// </summary>
        public static readonly ExceptionCustomCodes Conflict = new ExceptionCustomCodes(409, "Conflict");

        /// <summary>
        /// Indicates the resource is not accessible.
        /// </summary>
        public static readonly ExceptionCustomCodes Forbidden = new ExceptionCustomCodes(403, "Forbidden");

        /// <summary>
        /// Indicates a timeout error.
        /// </summary>
        public static readonly ExceptionCustomCodes Timeout = new ExceptionCustomCodes(408, "Timeout");

        /// <summary>
        /// Indicates the item is not authorized.
        /// </summary>
        public static readonly ExceptionCustomCodes Unauthorized = new ExceptionCustomCodes(401, "Unauthorized");

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCustomCodes"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="displayName">The display name.</param>
        public ExceptionCustomCodes(int value, string displayName) : base(value, displayName)
        {
        }
    }
}
