using Azure.Storage.Sas;

namespace Liquid.Adapter.AzureStorage
{
    /// <summary>
    /// Definition of BlobStorage integration service.
    /// </summary>
    public interface ILiquidBlobStorageAdapter
    {
        /// <summary>
        /// Upload a specific blob.
        /// </summary>
        /// <param name="data">Blob content.</param>
        /// <param name="name">Blob path.</param>
        /// <param name="containerName">Blob container name.</param>
        /// <param name="tags">Blob list of tags.</param>
        Task<string> UploadBlob(byte[] data, string name, string containerName, IDictionary<string, string>? tags = null);

        /// <summary>
        /// Remove blob by id.
        /// </summary>
        /// <param name="id">blob name.</param>
        /// <param name="containerName">Blob container name.</param>
        Task Delete(string id, string containerName);

        /// <summary>
        /// Filter blob by tags and remove them.
        /// </summary>
        /// <param name="tags">Tags for filter.</param>
        /// <param name="containerName">Blob container name.</param>
        Task DeleteByTags(IDictionary<string, string> tags, string containerName);

        /// <summary>
        /// Get all blobs from a container.
        /// </summary>
        /// <param name="containerName">Blob container name.</param>
        /// <returns>List of <see cref="LiquidBlob"/>.</returns>
        Task<List<LiquidBlob>> GetAllBlobs(string containerName);

        /// <summary>
        /// Filter blobs by tags.
        /// </summary>
        /// <param name="tags">Tags for filter.</param>
        /// <param name="containerName">Blob container name.</param>
        /// <returns>List of <see cref="LiquidBlob"/>.</returns>
        Task<List<LiquidBlob>> ReadBlobsByTags(IDictionary<string, string> tags, string containerName);

        /// <summary>
        /// Dowload a specific blob.
        /// </summary>
        /// <param name="blobName">Blob Id.</param>
        /// <param name="containerName">Blob container name.</param>
        /// <returns><see cref="LiquidBlob"/>.</returns>
        Task<LiquidBlob> ReadBlobsByName(string blobName, string containerName);

        /// <summary>
        /// generates a Blob Shared Access Signature (SAS) Uri
        /// based on the parameters passed. The SAS is signed by the shared key 
        /// credential of the client.
        /// </summary>
        /// <param name="blobName">The id of the blob.</param>
        /// <param name="containerName">Name of the container where the blob is stored.</param>
        /// <param name="expiresOn">The time at which the shared access signature becomes invalid.
        /// This field must be omitted if it has been specified in an
        /// associated stored access policy.</param>
        /// <param name="permissions">The permissions associated with the shared access signature. The
        /// user is restricted to operations allowed by the permissions.</param>
        /// <returns>Blob sas uri absolute path.</returns>
        string? GetBlobSasUri(string blobName, string containerName, DateTimeOffset expiresOn, BlobContainerSasPermissions permissions);
    }
}
