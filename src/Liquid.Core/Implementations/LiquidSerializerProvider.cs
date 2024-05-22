using Liquid.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Liquid.Core.Implementations
{
    ///<inheritdoc/>
    /// <summary>
    /// Initilize a new instance of <see cref="LiquidSerializerProvider"/>
    /// </summary>
    /// <param name="serializers"></param>
    public class LiquidSerializerProvider(IEnumerable<ILiquidSerializer> serializers) : ILiquidSerializerProvider
    {
        private readonly IEnumerable<ILiquidSerializer> _serializers = serializers ?? throw new ArgumentNullException(nameof(serializers));

        ///<inheritdoc/>
        public ILiquidSerializer GetSerializerByType(Type serializerType)
        {
            return _serializers.SingleOrDefault(x => x.GetType() == serializerType);
        }
    }
}
