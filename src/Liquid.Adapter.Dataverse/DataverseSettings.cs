using System.Diagnostics.CodeAnalysis;

namespace Liquid.Adapter.Dataverse
{
    /// <summary>
    /// Set of dataverse connection configs.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DataverseSettings
    {
        /// <summary>
        /// User Id to use.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// User secret to use.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Dataverse Instance to connect too.
        /// </summary>
        public string Url { get; set; }
    }

}
