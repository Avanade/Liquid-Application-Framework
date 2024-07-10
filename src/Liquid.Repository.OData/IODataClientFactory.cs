using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Repository.OData
{
    /// <summary>
    /// Defines an object with the ability to create an OData client.
    /// </summary>
    public interface IODataClientFactory
    {
        /// <summary>
        /// Create an OData client.
        /// </summary>
        /// <param name="entityName">The entity name.</param>
        IODataClient CreateODataClientAsync(string entityName);
    }
}
