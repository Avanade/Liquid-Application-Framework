using Liquid.Core.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Liquid.Core.Localization
{
    /// <summary>
    /// Occurs when it's not possible to read a resource.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class LocalizationException : LiquidException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationException"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="innerException">The inner exception.</param>
        public LocalizationException(string key, Exception innerException) : base($"Unable to read resource from key: {key}, please see inner exception.", innerException)
        {
        }

        ///<inheritdoc/>
        protected LocalizationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}