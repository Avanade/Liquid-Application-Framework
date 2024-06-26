using Microsoft.Extensions.Options;
using NSubstitute;

namespace Liquid.Repository.OData.Tests
{
    public class ODataClientFactoryTests
    {
        private IODataClientFactory _sut;
        private IOptions<ODataOptions> _options;

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

            _sut = new ODataClientFactory(_options);
        }


        [Fact]
        public void ODataClientFactory_WhenEntityNameIsNotFound_ThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _sut.CreateODataClientAsync("TestEntities2", "token"));
        }

        [Fact]
        public void ODataClientFactory_WhenEntityNameIsNull_ThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _sut.CreateODataClientAsync(null, "token"));
        }

        [Fact]
        public void OdataClientFactory_WhenValidateCertIsFalse_ReturnClient()
        {
            var client = _sut.CreateODataClientAsync("TestEntities", "token");

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

            var sut = new ODataClientFactory(_options);

            var client = sut.CreateODataClientAsync("TestEntities", "token");

            Assert.NotNull(client);
        }

    }
}