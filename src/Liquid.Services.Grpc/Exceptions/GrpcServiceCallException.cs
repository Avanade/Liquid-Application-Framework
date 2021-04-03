using System;
using Liquid.Core.Exceptions;

namespace Liquid.Services.Grpc.Exceptions
{
    /// <summary>
    /// Occurs when a grpc call throws an exception.
    /// </summary>
    /// <seealso cref="Liquid.Core.Exceptions.LightException" />
    public class GrpcServiceCallException : LightException
    {
        private const string ExceptionMessage = "An error has occurred while executing grpc call {0}.";

        /// <summary>
        /// Initializes a new instance of the <see cref="GrpcServiceCallException"/> class.
        /// </summary>
        /// <param name="grpcCall">The GRPC call.</param>
        /// <param name="exception">The exception.</param>
        public GrpcServiceCallException(string grpcCall, Exception exception) : base(string.Format(ExceptionMessage, grpcCall), exception)
        {
        }
    }
}