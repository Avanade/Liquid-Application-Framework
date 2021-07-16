using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using System;
using System.Collections.Generic;
using Xunit;

namespace Liquid.Core.Tests
{
    public class LiquidSerializerProviderTest
    {
        private ILiquidSerializerProvider _sut;

        private List<ILiquidSerializer> _serializers;

        public LiquidSerializerProviderTest()
        {
            _serializers = new List<ILiquidSerializer>();

            _serializers.Add(new LiquidJsonSerializer());

            _sut = new LiquidSerializerProvider(_serializers);
        }

        [Fact]
        public void GetSerializerByType_WhenServiceTypeExists_ReturnService()
        {
            var result = _sut.GetSerializerByType(typeof(LiquidJsonSerializer));

            Assert.NotNull(result);
            Assert.Equal(typeof(LiquidJsonSerializer), result.GetType());
        }

        [Fact]
        public void GetSerializerByType_WhenServiceTypeDoesntExists_ReturnNull()
        {
            var result = _sut.GetSerializerByType(typeof(LiquidXmlSerializer));

            Assert.Null(result);
        }

        [Fact]
        public void Ctor_WhenArgumentIsNull_ThrowException()
        {
            IEnumerable<ILiquidSerializer> serializers = default;

            Assert.Throws<ArgumentNullException>(() => new LiquidSerializerProvider(serializers));
        }
    }
}
