using Liquid.Core.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Repository.Exceptions
{
    /// <summary>
    /// Occurs when the transaction is not started.
    /// </summary>
    /// <seealso cref="Liquid.Core.Exceptions.LightException" />
    [ExcludeFromCodeCoverage]
    public class UnitOfWorkTransactionNotStartedException : LightException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkTransactionNotStartedException"/> class.
        /// </summary>
        public UnitOfWorkTransactionNotStartedException() :
            base("The transaction has been not started. Please start the transaction")
        {
        }
    }
}
