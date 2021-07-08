using Liquid.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Liquid.Core.Implementations
{
    ///<inheritdoc/>
    public class LiquidSerializerProvider : ILiquidSerializerProvider
    {
        private readonly IEnumerable<ILiquidSerializer> _serializers;

        /// <summary>
        /// Initilize a new instance of <see cref="LiquidSerializerProvider"/>
        /// </summary>
        /// <param name="serializers"></param>
        public LiquidSerializerProvider(IEnumerable<ILiquidSerializer> serializers)
        {
            _serializers = serializers ?? throw new ArgumentNullException(nameof(serializers));
        }

        ///<inheritdoc/>
        public ILiquidSerializer GetSerializerByType(Type serializerType)
        {
            return _serializers.SingleOrDefault(x => x.GetType() == serializerType);
        }
    }
}
