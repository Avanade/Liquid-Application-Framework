using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Liquid.WebApi.Grpc.Configuration
{
    /// <summary>
    /// Api settings class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApiSettings
    {
        private bool? _trackRequests;
        private bool? _showDetailedException;

        /// <summary>
        /// Gets or sets the use custom request tracking.
        /// </summary>
        /// <value>
        /// The use custom request tracking.
        /// </value>
        [JsonProperty("trackRequests")]
        public bool? TrackRequests
        {
            get => _trackRequests ?? true;
            set => _trackRequests = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show detailed exception].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show detailed exception]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("showDetailedException")]
        public bool? ShowDetailedException
        {
            get => _showDetailedException ?? false;
            set => _showDetailedException = value;
        }
    }
}