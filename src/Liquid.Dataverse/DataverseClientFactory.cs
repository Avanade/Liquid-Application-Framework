using Microsoft.Extensions.Options;
using Microsoft.PowerPlatform.Dataverse.Client;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Dataverse
{
    ///<inheritdoc/>
    public class DataverseClientFactory : IDataverseClientFactory
    {
        private readonly IOptions<DataverseSettings> _options;

        /// <summary>
        /// Initialize a new instance of <see cref="DataverseClientFactory"/>
        /// </summary>
        /// <param name="options">Configuration settigs.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DataverseClientFactory(IOptions<DataverseSettings> options)
        {
            ArgumentNullException.ThrowIfNull(options);

            _options = options;
        }

        ///<inheritdoc/>   
        [ExcludeFromCodeCoverage]
        public IOrganizationServiceAsync GetClient()
        {
            var settings = _options.Value;

            var connectionString = string.Format("AuthType=ClientSecret;url={0};ClientId={1};ClientSecret={2};", settings.Url, settings.ClientId, settings.ClientSecret);

            var service = new ServiceClient(connectionString);

            return service;
        }
    }
}
