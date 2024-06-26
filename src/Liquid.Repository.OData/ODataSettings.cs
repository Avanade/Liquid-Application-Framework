namespace Liquid.Repository.OData
{
    /// <summary>
    /// Odata configurations set.
    /// </summary>
    public class ODataSettings
    {
        /// <summary>
        /// Name of entity to be configured.
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Base URL of the Odata service.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Indicates if the Odata service requires certificate validations.
        /// </summary>
        public bool ValidateCert { get; set; } = false;
    }

    /// <summary>
    /// Odata configuration options.
    /// </summary>
    public class ODataOptions
    {
        /// <summary>
        /// List of Odata options set.
        /// </summary>
        public List<ODataSettings> Settings { get; set; }
    }
}
