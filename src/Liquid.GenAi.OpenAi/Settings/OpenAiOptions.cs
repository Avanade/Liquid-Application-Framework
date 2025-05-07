using System.Diagnostics.CodeAnalysis;

namespace Liquid.GenAi.OpenAi.Settings
{
    /// <summary>
    /// The options for chat completions request.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class OpenAiOptions
    {
        /// <summary>
        /// Client connection list to use for create OpenAi clients.
        /// </summary>
        public List<OpenAiSettings> Settings { get; set; }
    }

    /// <summary>
    /// The settings for chat completions request.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class OpenAiSettings
    {
        /// <summary>
        /// Client connection alias.
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// The URI for an GenAI resource as retrieved from, for example, Azure Portal.
        ///This should include protocol and hostname.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Key to use to authenticate with the service.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The deployment name to use for a chat completions request.
        /// </summary>
        public string DeploymentName { get; set; }

        /// <summary>
        /// The maximum number of retries to allow.
        /// </summary>
        public int MaxRetries { get; set; } = 0;        
    }
}
