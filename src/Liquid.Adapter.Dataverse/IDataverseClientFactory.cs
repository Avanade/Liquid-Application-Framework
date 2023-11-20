using Microsoft.PowerPlatform.Dataverse.Client;

namespace Liquid.Adapter.Dataverse
{
    /// <summary>
    /// Defines Dataverse <see cref="ServiceClient"/> provider.
    /// </summary>
    public interface IDataverseClientFactory
    {
        /// <summary>
        /// Initialize a new instance of <see cref="ServiceClient"/>
        /// connected too <see cref="DataverseSettings"/> .
        /// </summary>
        IOrganizationServiceAsync GetClient();
    }
}