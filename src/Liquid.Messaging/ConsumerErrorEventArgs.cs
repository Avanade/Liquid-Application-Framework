﻿using System;

namespace Liquid.Messaging
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