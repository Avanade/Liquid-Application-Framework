using Azure;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Liquid.Adapter.AzureStorage
{
    ///<inheritdoc/>
    [ExcludeFromCodeCoverage]
    public class BlobStorageAdapter : ILiquidBlobStorageAdapter
    {
        private readonly IBlobClientFactory _factory;

        /// <summary>
        /// Initialize a new instance of <see cref="BlobStorageAdapter"/>
        /// </summary>
        /// <param name="factory"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public BlobStorageAdapter(IBlobClientFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));

            _factory.SetContainerClients();
        }

        ///<inheritdoc/>
        public async Task DeleteByTags(IDictionary<string, string> tags, string containerName)
        {
            var client = _factory.GetContainerClient(containerName);

            var stringFilter = string.Empty;

            foreach (var tag in tags)
            {
                stringFilter += @$"""{tag.Key}"" = '{tag.Value}' AND ";
            }

            stringFilter = stringFilter.Substring(0, stringFilter.Length - 4);

            await foreach (TaggedBlobItem blobItem in client.FindBlobsByTagsAsync(stringFilter))
            {
                var blockBlob = client.GetBlockBlobClient(blobItem.BlobName);

                await blockBlob.DeleteAsync();
            };
        }

        ///<inheritdoc/>
        public async Task<List<LiquidBlob>> GetAllBlobs(string containerName)
        {
            var client = _factory.GetContainerClient(containerName);

            var results = new List<LiquidBlob>();

            await foreach (var blobItem in client.GetBlobsAsync())
            {
                var blockBlob = client.GetBlockBlobClient(blobItem.Name);
                var blob = await blockBlob.DownloadContentAsync();

                var item = new LiquidBlob
                {
                    Blob = blob.Value.Content.ToArray(),
                    Tags = blockBlob.GetTags().Value.Tags,
                    Name = blobItem.Name,
                    AbsoluteUri = blockBlob.Uri.AbsoluteUri
                };
                results.Add(item);
            }

            return results;
        }

        ///<inheritdoc/>
        public async Task Delete(string id, string containerName)
        {
            var client = _factory.GetContainerClient(containerName);

            var blobClient = client.GetBlobClient(id);

            await blobClient.DeleteAsync();
        }

        ///<inheritdoc/>
        public async Task<List<LiquidBlob>> ReadBlobsByTags(IDictionary<string, string> tags, string containerName)
        {
            var client = _factory.GetContainerClient(containerName);

            var stringFilter = string.Empty;
            foreach (var tag in tags)
            {
                stringFilter += @$"""{tag.Key}"" = '{tag.Value}' AND ";
            }
            stringFilter = stringFilter.Substring(0, stringFilter.Length - 4);

            var results = new List<LiquidBlob>();
            await foreach (TaggedBlobItem blobItem in client.FindBlobsByTagsAsync(stringFilter))
            {
                var blockBlob = client.GetBlockBlobClient(blobItem.BlobName);
                var blob = await blockBlob.DownloadContentAsync();
                var item = new LiquidBlob
                {
                    Blob = blob.Value.Content.ToArray(),
                    Tags = blockBlob.GetTags().Value.Tags,
                    Name = blobItem.BlobName,
                    AbsoluteUri = blockBlob.Uri.AbsoluteUri
                };
                results.Add(item);
            }
            return results;
        }

        ///<inheritdoc/>
        public async Task<string> UploadBlob(byte[] data, string name, string containerName, IDictionary<string, string>? tags = null)
        {
            var client = _factory.GetContainerClient(containerName);

            var blockBlob = client.GetBlockBlobClient(name);

            var options = new BlobUploadOptions()
            {
                Tags = tags
            };
            await blockBlob.UploadAsync(new MemoryStream(data), options);

            return blockBlob.Uri.AbsoluteUri;
        }

        ///<inheritdoc/>
        public async Task<LiquidBlob> ReadBlobsByName(string blobName, string containerName)
        {
            try
            {
                var client = _factory.GetContainerClient(containerName);
                var blockBlob = client.GetBlockBlobClient(blobName);
                var blob = await blockBlob.DownloadContentAsync();
                var item = new LiquidBlob
                {
                    Blob = blob.Value.Content.ToArray(),
                    Tags = blockBlob.GetTags().Value.Tags,
                    Name = blobName,
                    AbsoluteUri = blockBlob.Uri.AbsoluteUri
                };

                return item;
            }
            catch (RequestFailedException storageRequestFailedException)
                when (storageRequestFailedException.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                return null;
            }
            catch (Exception)
            {
                throw;
            }

        }

        ///<inheritdoc/>
        public string? GetBlobSasUri(string blobName, string containerName, DateTimeOffset expiresOn, BlobContainerSasPermissions permissions)
        {
            var blobClient = _factory.GetContainerClient(containerName);

            var blockBlob = blobClient.GetBlockBlobClient(blobName);

            if (!blobClient.CanGenerateSasUri)
            {
                return null;
            }

            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = blobClient.Name,
                BlobName = blobName,
                Resource = "b",
            };

            sasBuilder.ExpiresOn = expiresOn;
            sasBuilder.SetPermissions(permissions);

            var sasURI = blockBlob.GenerateSasUri(sasBuilder);

            return sasURI.AbsoluteUri;
        }
    }
}
