using Liquid.Core.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Liquid.Repository.Exceptions
{
    /// <summary>
    /// Occurs when a Repository database throw an error.
    /// </summary>
    public class RepositoryDatabaseContextException : LightException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryDatabaseContextException"/> class.
        /// </summary>
        /// <param name="message">Error message custom text.</param>
        /// <param name="innerException">Exception throwed by the client.</param>
        public RepositoryDatabaseContextException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
