using System.Diagnostics.CodeAnalysis;

namespace Liquid.Core.Entities
{
    /// <summary>
    /// Client dictionary to store client instances.
    /// </summary>
    /// <typeparam name="T">Type of client service.</typeparam>
    [ExcludeFromCodeCoverage]
    public class ClientDictionary<T>
    {
        /// <summary>
        /// Number of executions for this client.
        /// </summary>
        public int Executions { get; set; } = 1;


        /// <summary>
        /// Client connection alias.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Client instance.
        /// </summary>
        public T Client { get; set; }

        /// <summary>
        /// Initialize a new instance of <see cref="ClientDictionary{T}"/>.
        /// </summary>
        /// <param name="clientId">Client connection alias.</param>
        /// <param name="client">Client instance.</param>
        public ClientDictionary(string clientId, T client)
        {
            ClientId = clientId;
            Client = client;
        }
    }
}
