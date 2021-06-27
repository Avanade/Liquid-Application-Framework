using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Liquid.Core.Exceptions;

namespace Liquid.Core.Localization
{
    /// <summary>
    /// Occurs when it's not possible to read the resources collection from data source.
    /// </summary>
    /// <seealso cref="Exception" />
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class LocalizationReaderException : LiquidException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationReaderException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public LocalizationReaderException(Exception innerException): base("An error occurred while reading the resource collection from datasource.", innerException)
        {
        }

        ///<inheritdoc/>
        protected LocalizationReaderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}