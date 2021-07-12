using Liquid.Core.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Liquid.Repository.Exceptions
{
    /// <summary>
    /// Occurs when the transaction is not started.
    /// </summary>
    /// <seealso cref="Liquid.Core.Exceptions.LightException" />
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class UnitOfWorkTransactionNotStartedException : LiquidException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkTransactionNotStartedException"/> class.
        /// </summary>
        public UnitOfWorkTransactionNotStartedException() :
            base("The transaction has been not started. Please start the transaction")
        {
        }

        ///<inheritdoc/>
        public UnitOfWorkTransactionNotStartedException(string message) : base(message)
        {
        }

        ///<inheritdoc/>
        public UnitOfWorkTransactionNotStartedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        ///<inheritdoc/>
        protected UnitOfWorkTransactionNotStartedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
