using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace Liquid.Storage.AzureStorage
{
    ///<inheritdoc/>
    public class BlobClientFactory : IBlobClientFactory
    {
        private readonly StorageSettings _options;
        private readonly List<BlobContainerClient> _clients = new List<BlobContainerClient>();

        ///<inheritdoc/>
        public IList<BlobContainerClient> Clients => _clients;

        /// <summary>
        /// Inicialize a new instance of <see cref="BlobClientFactory"/>
        /// </summary>
        /// <param name="options">Configurations set.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public BlobClientFactory(IOptions<StorageSettings>? options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        ///<inheritdoc/>
        public void SetContainerClients()
        {
            if (_options.Containers.Count == 0)
                throw new ArgumentNullException(nameof(_options));

            foreach (var container in _options.Containers)
            {
                var client = new BlobContainerClient(container.ConnectionString, container.ContainerName);

                _clients.Add(client);
            }
        }

        ///<inheritdoc/>
        public BlobContainerClient GetContainerClient(string containerName)
        {
            var client = _clients.FirstOrDefault(x => x.Name == containerName);

            if (client == null)
            {
                throw new ArgumentException($"Container named {containerName} not found.");
            }

            return client;
        }
    }
}
