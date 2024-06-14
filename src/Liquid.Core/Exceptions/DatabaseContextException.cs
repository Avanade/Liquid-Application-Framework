﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Liquid.Core.Exceptions
{
    /// <summary>
    /// Occurs when a Repository database throw an error.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class DatabaseContextException : LiquidException
    {
        ///<inheritdoc/>
        public DatabaseContextException(string message) : base(message)
        {
        }

        ///<inheritdoc/>
        protected DatabaseContextException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseContextException"/> class.
        /// </summary>
        /// <param name="message">Error message custom text.</param>
        /// <param name="innerException">Exception throwed by the client.</param>
        public DatabaseContextException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
