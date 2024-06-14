using System;

namespace Liquid.Core.Entities
{
    /// <summary>
    /// Arguments for processing errors occurred during process execution.
    /// </summary>
    public class ConsumerErrorEventArgs
    {
        /// <summary>
        ///   Represents errors that occur during process execution.
        /// </summary>
        public Exception Exception { get; set; }
    }
}