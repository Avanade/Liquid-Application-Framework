using Azure.Storage.Blobs;

namespace Liquid.Adapter.AzureStorage
{
    /// <summary>
    /// <see cref="BlobContainerClient"/> instances factory. 
    /// </summary>
    public interface IBlobClientFactory
    {
        /// <summary>
        /// List of instances of <see cref="BlobContainerClient"/>.
        /// </summary>
        IList<BlobContainerClient> Clients { get; }

        /// <summary>
        /// Initialize an instance of <see cref="BlobContainerClient"/> 
        /// for each container on the <see cref="StorageSettings"/> and 
        /// add to <see cref="Clients"/>.
        /// </summary>
        List<BlobContainerClient> SetContainerClients();

        /// <summary>
        /// Get an instance of <see cref="BlobContainerClient"/>
        /// by name.
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        BlobContainerClient GetContainerClient(string containerName);
    }
}
