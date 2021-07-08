using System;

namespace Liquid.Core.Interfaces
{
    /// <summary>
    /// Generates a new instance of Serializer Services <see cref="ILiquidSerializer"/>
    /// </summary>
    public interface ILiquidSerializerProvider
    {
        /// <summary>
        /// Gets instance of validation service by the type entered in <paramref name="serializerType"/>.
        /// </summary>
        /// <param name="serializerType">Type of serializer to get.</param>
        /// <returns></returns>
        ILiquidSerializer GetSerializerByType(Type serializerType);
    }
}
