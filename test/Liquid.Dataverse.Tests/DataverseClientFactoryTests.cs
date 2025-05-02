using Microsoft.Extensions.Options;
using NSubstitute;

namespace Liquid.Dataverse.Tests
{
    public class DataverseClientFactoryTests
    {
        private readonly IDataverseClientFactory _sut;
        private readonly IOptions<DataverseSettings> _options;

        public DataverseClientFactoryTests()
        {
            _options = Substitute.For<IOptions<DataverseSettings>>();
            _options.Value.ReturnsForAnyArgs(new DataverseSettings() { ClientId = "4erewgewgh", ClientSecret = "greggrbnte", Url = "https://test" });
            _sut = new DataverseClientFactory(_options);
        }


        [Fact]
        public void Ctor_WhenOptionsIsNull_ThenReturnArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DataverseClientFactory(null));
        }

        [Fact]
        public void Ctor_WhenOptionsExists_ThenReturnDataverseClientFactoryInstance()
        {
            var result = new DataverseClientFactory(_options);
            Assert.NotNull(result);
            Assert.IsType<DataverseClientFactory>(result);
        }
    }
}