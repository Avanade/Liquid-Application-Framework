using Liquid.Core.Exceptions;
using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Liquid.Core.Tests.Mocks;
using Xunit;

namespace Liquid.Core.Tests
{
    public class LiquidXmlSerializerTest
    {
        private ILiquidSerializer _sut;

        public LiquidXmlSerializerTest()
        {
            _sut = new LiquidXmlSerializer();
        }

        [Fact]
        public void Serialize_WhenContentTyped_ReturnXmlString()
        {
            var content = new MockSerializeObject(1, "2");

            var result = _sut.Serialize(content);

            Assert.NotNull(result);
        }

        [Fact]
        public void Serialize_WhenContentIsAnonymous_ThrowException()
        {
            var content = new { stringProperty = "1", intPropery = 2 };

            Assert.Throws<SerializerFailException>(() => _sut.Serialize(content));
        }
    }
}
