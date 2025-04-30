using Microsoft.Extensions.Options;
using NSubstitute;

namespace Liquid.Storage.AzureStorage.Tests
{
    public class BlobClientFactoryTests
    {

        private readonly IBlobClientFactory _sut;
        private readonly IOptions<StorageSettings> _options;

        public BlobClientFactoryTests()
        {
            _options = Substitute.For<IOptions<StorageSettings>>();

            var settings = new StorageSettings();
            settings.Containers.Add(new ContainerSettings()
            {
                ContainerName = "test",
                ConnectionString = "testestestes"
            });

            _options.Value.ReturnsForAnyArgs(settings);
            _sut = new BlobClientFactory(_options);
        }


        [Fact]
        public void Ctor_WhenOptionsIsNull_ThenReturnArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new BlobClientFactory(null));
        }

        [Fact]
        public void Ctor_WhenOptionsExists_ThenBlobClientFactoryInstance()
        {
            var result = new BlobClientFactory(_options);
            Assert.NotNull(result);
            Assert.IsType<BlobClientFactory>(result);
        }
        [Fact]
        public void SetContainerClients_WhenOptionsNotSet_ThenThrowArgumentNullException()
        {
            var options = Substitute.For<IOptions<StorageSettings>>();
            options.Value.ReturnsForAnyArgs(new StorageSettings());

            var sut = new BlobClientFactory(options);

            Assert.Throws<ArgumentNullException>(() => sut.SetContainerClients());

        }

        [Fact]
        public void SetContainerClients_WhenContainerNameIsInvalid_ThenThrowFormatException()
        {
            Assert.True(_sut.Clients.Count == 0);
            Assert.Throws<FormatException>(() => _sut.SetContainerClients());
        }

        [Fact]
        public void GetContainerClient_WhenClientDoesntExists_ThenThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _sut.GetContainerClient("test"));
        }
    }
}