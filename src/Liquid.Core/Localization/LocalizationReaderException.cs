using System;
using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Exceptions;

namespace Liquid.Core.Localization
{
    /// <summary>
    /// Occurs when it's not possible to read the resources collection from data source.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class LocalizationReaderException : LightException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationReaderException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public LocalizationReaderException(Exception innerException): base("An error occurred while reading the resource collection from datasource.", innerException)
        {
        }
    }
}