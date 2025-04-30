using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Liquid.Core.Entities;
using NSubstitute;
using System.Text;

namespace Liquid.Storage.AzureStorage.Tests
{
    public class LiquidStorageAzureTests
    {
        private readonly LiquidStorageAzure _sut;
        private BlobContainerClient _blobContainerClient = Substitute.For<BlobContainerClient>();
        private BlockBlobClient _blockBlobClient = Substitute.For<BlockBlobClient>();
        public LiquidStorageAzureTests()
        {
            var clientFactory = Substitute.For<IBlobClientFactory>();

            _blobContainerClient.GetBlockBlobClient(Arg.Any<string>()).Returns(_blockBlobClient);

            clientFactory.GetContainerClient(Arg.Any<string>()).Returns(_blobContainerClient);

            _sut = new LiquidStorageAzure(clientFactory);
        }

        [Fact]
        public void Ctor_WhenClientFactoryIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            IBlobClientFactory clientFactory = null;
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new LiquidStorageAzure(clientFactory));
        }

        [Fact]
        public void Ctor_WhenClientFactoryIsNotNull_DoesNotThrow()
        {
            // Arrange
            var clientFactory = Substitute.For<IBlobClientFactory>();
            // Act & Assert
            var exception = Record.Exception(() => new LiquidStorageAzure(clientFactory));
            Assert.Null(exception);
        }

        [Fact]
        public async void DeleteByTags_WhenTagsAreValid_DoesNotThrow()
        {
            // Arrange
            var tags = new Dictionary<string, string>
            {
                { "tag1", "value1" },
                { "tag2", "value2" }
            };
            var containerName = "test-container";
            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _sut.DeleteByTags(tags, containerName));
            Assert.Null(exception);
        }

        [Fact]
        public async void GetAllBlobs_WhenContainerNameIsValid_ReturnsListOfLiquidBlob()
        {
            // Arrange
            var containerName = "test-container";
            // Act
            var result = await _sut.GetAllBlobs(containerName);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<LiquidBlob>>(result);
        }
        [Fact]
        public async void ReadBlobsByName_WhenBlobNameIsValid_ReturnsLiquidBlob()
        {
            // Arrange
            var blobName = "test-blob";
            var containerName = "test-container";

            _blockBlobClient.DownloadContentAsync().Returns(Response.FromValue(
                BlobsModelFactory.BlobDownloadResult(new BinaryData("test-blob")), null
            ));

            _blockBlobClient.GetTagsAsync().Returns(Response.FromValue(
                BlobsModelFactory.GetBlobTagResult(new Dictionary<string, string> { { "tag1", "value1" } }), null
            ));

            _blockBlobClient.Uri.Returns(new Uri("https://test.blob.core.windows.net/test-blob"));

            // Act
            var result = await _sut.ReadBlobsByName(blobName, containerName);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<LiquidBlob>(result);
        }

        [Fact]
        public async void ReadBlobsByName_WhenRequestFail_ThrowsException()
        {
            // Arrange
            var blobName = "test-blob";
            var containerName = "test-container";
            _blockBlobClient.DownloadContentAsync().Returns(Task.FromException<Response<BlobDownloadResult>>(new Exception("Test exception")));
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _sut.ReadBlobsByName(blobName, containerName));
        }

        [Fact]
        public async void ReadBlobsByName_WhenRequestFailedException_ReturnNull()
        {
            // Arrange
            var blobName = "test-blob";
            var containerName = "test-container";
            _blockBlobClient.DownloadContentAsync().Returns(Task.FromException<Response<BlobDownloadResult>>(new RequestFailedException(0, "BlobNotFound", "BlobNotFound", new Exception())));
            // Act
            var result = await _sut.ReadBlobsByName(blobName, containerName);
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void UploadBlob_WhenDataIsValid_ReturnsBlobUri()
        {
            // Arrange
            var data = Encoding.UTF8.GetBytes("test data");
            var name = "test-blob";
            var containerName = "test-container";
            var tags = new Dictionary<string, string>
            {
                { "tag1", "value1" },
                { "tag2", "value2" }
            };
            _blockBlobClient.UploadAsync(Arg.Any<Stream>(), Arg.Any<BlobUploadOptions>()).Returns(Response.FromValue(
                BlobsModelFactory.BlobContentInfo(ETag.All, DateTimeOffset.UtcNow, data, "", "", "", data.Length), null));

            _blockBlobClient.Uri.Returns(new Uri("https://test.blob.core.windows.net/test-blob"));
            // Act
            var result = await _sut.UploadBlob(data, name, containerName, tags);
            // Assert
            Assert.NotNull(result);

        }

        [Fact]
        public async void UploadBlob_WhenDataIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            byte[] data = null;
            var name = "test-blob";
            var containerName = "test-container";
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.UploadBlob(data, name, containerName));
        }

        [Fact]

        public void GetBlobSasUri_WhenBlobNameIsValid_ReturnsSasUri()
        {
            // Arrange
            var blobName = "test-blob";
            var containerName = "test-container";
            var expiresOn = DateTimeOffset.UtcNow.AddHours(1);
            var permissions = "Read";
            _blobContainerClient.CanGenerateSasUri.Returns(true);
            _blockBlobClient.GenerateSasUri(Arg.Any<BlobSasBuilder>()).Returns(new Uri("https://test.blob.core.windows.net/test-blob?sv=2020-08-04&ss=b&srt=sco&sp=r&se=2023-10-01T00:00:00Z&st=2023-09-01T00:00:00Z&spr=https,http&sig=signature"));

            // Act
            var result = _sut.GetBlobSasUri(blobName, containerName, expiresOn, permissions);
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetBlobSasUri_WhenCantGenerateSasUri_ReturnsNull()
        {
            // Arrange
            var blobName = "test-blob";
            var containerName = "test-container";
            var expiresOn = DateTimeOffset.UtcNow.AddHours(1);
            var permissions = "Read";
            _blobContainerClient.CanGenerateSasUri.Returns(false);
            // Act
            var result = _sut.GetBlobSasUri(blobName, containerName, expiresOn, permissions);
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Delete_WhenBlobNameIsValid_DoesNotThrow()
        {
            // Arrange
            var blobName = "test-blob";
            var containerName = "test-container";

            _blobContainerClient.GetBlobClient(Arg.Any<string>()).Returns(Substitute.For<BlobClient>());
            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _sut.Delete(blobName, containerName));

            Assert.Null(exception);
        }

        [Fact]
        public async void ReadBlobsByTags_WhenTagsAreValid_ReturnsListOfLiquidBlob()
        {
            // Arrange
            var tags = new Dictionary<string, string>
            {
                { "tag1", "value1" },
                { "tag2", "value2" }
            };
            var containerName = "test-container";
            // Act
            var result = await _sut.ReadBlobsByTags(tags, containerName);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<LiquidBlob>>(result);
        }
    }
}
