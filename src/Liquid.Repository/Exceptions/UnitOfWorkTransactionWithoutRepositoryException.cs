using Liquid.Core.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Liquid.Repository.Exceptions
{
    /// <summary>
    /// Occurs when the transaction is called before a repository method is called.
    /// </summary>
    /// <seealso cref="Liquid.Core.Exceptions.LightException" />
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class UnitofWorkTransactionWithoutRepositoryException : LightException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitofWorkTransactionWithoutRepositoryException"/> class.
        /// </summary>
        public UnitofWorkTransactionWithoutRepositoryException() :
            base("You need to get a repository first to start an transaction. Use 'GetRepository' method.")
        {
        }

        ///<inheritdoc/>
        public UnitofWorkTransactionWithoutRepositoryException(string message) : base(message)
        {
        }

        ///<inheritdoc/>
        public UnitofWorkTransactionWithoutRepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        ///<inheritdoc/>
        protected UnitofWorkTransactionWithoutRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
