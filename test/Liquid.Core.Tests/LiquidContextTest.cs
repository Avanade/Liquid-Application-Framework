using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Xunit;

namespace Liquid.Core.UnitTests
{
    public class LiquidContextTest
    {
        private ILiquidContext _sut;
        public LiquidContextTest()
        {
            _sut = new LiquidContext();
            _sut.Upsert("teste", 123);
        }

        [Fact]
        public void UpsertKey_WhenContextIsEmpty_KeyInserted()
        {
            var sut = new LiquidContext();

            sut.Upsert("teste", 123);

            Assert.True((int)sut.current["teste"] == 123);
        }

        [Fact]
        public void UpsertKey_WhenInsertNewKey_NewKeyInserted()
        {
            _sut.Upsert("case2", 456);

            Assert.True((int)_sut.current["teste"] == 123);
            Assert.True((int)_sut.current["case2"] == 456);
            Assert.True(_sut.current.Count == 2);
        }

        [Fact]
        public void UpsertKey_WhenUpdateKey_KeyUpdated()
        {
            _sut.Upsert("teste", 456);

            Assert.True((int)_sut.current["teste"] == 456);
            Assert.True(_sut.current.Count == 1);
        }

        [Fact]
        public void Get_WhenKeyExists_ReturnValue()
        {
            var result = _sut.Get("teste");

            Assert.NotNull(result);
        }

        [Fact]
        public void Get_WhenKeyDoesntExist_ReturnNull()
        {
            var result = _sut.Get("case3");

            Assert.Null(result);

        }

        [Fact]
        public void Get_WhenCurrentHasNoItens_ReturnNull()
        {
            var sut = new LiquidContext();

            var result = sut.Get("teste");

            Assert.Null(result);
        }
    }
}
