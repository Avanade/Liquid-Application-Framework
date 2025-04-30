namespace Liquid.Storage.AzureStorage
{
    /// <summary>
    /// Set of Azure Storage containers configs.
    /// </summary>
    public class StorageSettings
    {
        /// <summary>
        /// List of container settings.
        /// </summary>
        public List<ContainerSettings> Containers { get; set; } = new List<ContainerSettings>();
    }

    /// <summary>
    /// Set of a container connection configuration.
    /// </summary>
    public class ContainerSettings
    {
        /// <summary>
        /// A connection string includes the authentication information
        /// required for your application to access data in an Azure Storage
        /// account at runtime.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// The name of the blob container in the storage account to reference.
        /// </summary>
        public string ContainerName { get; set; }

    }
}
