using Liquid.Core.Interfaces;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Liquid.Repository.OData.Tests
{
    public class ODataClientFactoryTests
    {
        private IODataClientFactory _sut;
        private IOptions<ODataOptions> _options;
        private ILiquidContext _context;

        public ODataClientFactoryTests()
        {
            _options = Substitute.For<IOptions<ODataOptions>>();

            var settings = new ODataOptions()
            {
                Settings = new List<ODataSettings>()
                {
                    new ODataSettings()
                    {
                        BaseUrl = "http://localhost:5000",
                        EntityName = "TestEntities",
                    }
                }
            };

            _options.Value.Returns(settings);

            _context = Substitute.For<ILiquidContext>();
            _context.Get("OdataToken").Returns("token");
            _context.current.ContainsKey("OdataToken").Returns(true);

            _sut = new ODataClientFactory(_options, _context);
        }


        [Fact]
        public void ODataClientFactory_WhenEntityNameIsNotFound_ThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _sut.CreateODataClientAsync("TestEntities2"));
        }

        [Fact]
        public void ODataClientFactory_WhenEntityNameIsNull_ThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _sut.CreateODataClientAsync(null));
        }

        [Fact]
        public void OdataClientFactory_WhenValidateCertIsFalse_ReturnClient()
        {
            var client = _sut.CreateODataClientAsync("TestEntities");

            Assert.NotNull(client);
        }

        [Fact]
        public void OdataClientFactory_WhenValidateCertIsTrue_ReturnClient()
        {
            var settings = new ODataSettings()
            {
                BaseUrl = "http://localhost:5000",
                EntityName = "TestEntities",
                ValidateCert = true
            };

            _options.Value.Returns(new ODataOptions()
            {
                Settings = new List<ODataSettings>() { settings }
            });

            var sut = new ODataClientFactory(_options, _context);

            var client = sut.CreateODataClientAsync("TestEntities");

            Assert.NotNull(client);
        }

        [Fact]
        public void OdataClientFactory_WhenTokenIsNotSet_ThrowException()
        {
            var context = Substitute.For<ILiquidContext>();

            context.Get("OdataToken").Returns("");
            context.current.ContainsKey("OdataToken").Returns(true);

            var sut = new ODataClientFactory(_options, context);

            Assert.Throws<KeyNotFoundException>(() => sut.CreateODataClientAsync("TestEntities"));
        }

        [Fact]
        public void OdataClientFactory_WhenTokenIsNotSetInContext_ThrowException()
        {
            var context = Substitute.For<ILiquidContext>();

            context.Get("OdataToken").Returns(null);
            context.current.ContainsKey("OdataToken").Returns(false);

            var sut = new ODataClientFactory(_options, context);

            Assert.Throws<KeyNotFoundException>(() => sut.CreateODataClientAsync("TestEntities"));
        }

        [Fact]
        public void OdataClientFactory_WhenOptionsIsNull_ThrowException()
        {
            _options = null;

            Assert.Throws<ArgumentNullException>(() => new ODataClientFactory(_options, _context));
        }

        [Fact]
        public void OdataClientFactory_WhenContextIsNull_ThrowException()
        {
            _context = null;

            Assert.Throws<ArgumentNullException>(() => new ODataClientFactory(_options, _context));
        }
    }
}