using System;
using System.Collections.Generic;
using System.Text;

namespace Liquid.WebApi.Http.Entities
{
    /// <summary>
    /// Properties for response request that throws unexpected error.
    /// </summary>
    public class LiquidErrorResponse
    {
        /// <summary>
        /// Status code of error.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Detailed exception.
        /// </summary>
        public Exception Detailed { get; set; }
    }
}
