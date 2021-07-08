using System;
using System.Collections.Generic;
using System.Text;

namespace Liquid.Core.Interfaces
{
    /// <summary>
    /// Serialization
    /// </summary>
    public interface ILiquidSerializer
    {
        /// <summary>
        /// Serialize an object to string.
        /// </summary>
        /// <param name="content">object that shoud be serialized.</param>
        string Serialize<T>(T content);
    }
}
