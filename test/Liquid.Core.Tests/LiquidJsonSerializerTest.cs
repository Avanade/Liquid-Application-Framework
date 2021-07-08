using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Xunit;

namespace Liquid.Core.Tests
{
    public class LiquidJsonSerializerTest
    {
        private ILiquidSerializer _sut;

        public LiquidJsonSerializerTest()
        {
            _sut = new LiquidJsonSerializer();
        }

        [Fact]
        public void Serialize_WhenSerializeObject_ReturnJsonString()
        {
            var content = new { stringProperty = "1", intPropery = 2 };

            var result = _sut.Serialize(content);

            Assert.NotNull(result);
        }

    }
}
