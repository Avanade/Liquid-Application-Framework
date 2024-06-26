using Microsoft.Extensions.Options;
using Simple.OData.Client;

namespace Liquid.Repository.OData
{
    ///<inheritdoc/>
    public class ODataClientFactory : IODataClientFactory
    {
        private readonly IOptions<ODataOptions> _options;

        /// <summary>
        /// Initialize a new instance of <see cref="ODataClientFactory"/>
        /// </summary>
        /// <param name="options"></param>
        public ODataClientFactory(IOptions<ODataOptions> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        ///<inheritdoc/>
        public IODataClient CreateODataClientAsync(string entityName, string token)
        {
            var settings = _options?.Value?.Settings.FirstOrDefault(x => x.EntityName == entityName);

            if (settings == null)
                throw new ArgumentOutOfRangeException(nameof(entityName));

            var client = new ODataClient(GetODataSettings(settings, token));

            return client;
        }

        /// <summary>
        ///Initialize a new instance of <see cref="ODataClientSettings"/>
        /// </summary>
        /// <param name="settings">OData settings.</param>
        /// <param name="token"> Authorization token.</param>
        /// <returns></returns>
        private static ODataClientSettings GetODataSettings(ODataSettings? settings, string token)
        {
            var odataSettings = new ODataClientSettings(new Uri(settings.BaseUrl));

            odataSettings.BeforeRequest = (message) =>
            {
                message.Headers.Add("Authorization", token);
            };

            if (!settings.ValidateCert)
            {
                var handler = new HttpClientHandler();

                odataSettings.OnApplyClientHandler = (handler) =>
                {
                    handler.ServerCertificateCustomValidationCallback +=
                                (sender, certificate, chain, errors) =>
                                {
                                    return true;
                                };
                };
            }

            return odataSettings;
        }
    }
}
