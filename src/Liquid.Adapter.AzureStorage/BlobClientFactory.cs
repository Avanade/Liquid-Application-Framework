using Azure.Core;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace Liquid.Adapter.AzureStorage
{
    ///<inheritdoc/>
    public class BlobClientFactory : IBlobClientFactory
    {
        private readonly StorageSettings _options;
        private IList<BlobContainerClient> _clients = new List<BlobContainerClient>();

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
        public List<BlobContainerClient> SetContainerClients()
        {
            if(_options.Containers.Count  == 0)
                throw new ArgumentNullException(nameof(_options));

            var clients = new List<BlobContainerClient>();           

            foreach(var container in _options.Containers)
            {
                var client = new BlobContainerClient(container.ConnectionString,container.ContainerName);

                clients.Add(client);
            }

            return clients;
        }

        ///<inheritdoc/>
        public BlobContainerClient GetContainerClient(string containerName)
        {
            var client = _clients.FirstOrDefault(x => x.Name == containerName);

            if (client == null) { 
                throw new ArgumentException(nameof(containerName)); 
            }

            return client;
        }
    }
}
