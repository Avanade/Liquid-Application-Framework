﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Liquid.Core.Exceptions
{
    /// <summary>
    /// Liquid Base Custom Exception Class. Derived from <see cref="T:System.Exception"></see> class.
    /// </summary>
    /// <seealso cref="Exception" />
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class LiquidException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:LightException"></see> class.
        /// </summary>
        public LiquidException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:LightException"></see> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public LiquidException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:LightException"></see> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public LiquidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"Liquid Exception -> {base.ToString()}";
        }
    }
}