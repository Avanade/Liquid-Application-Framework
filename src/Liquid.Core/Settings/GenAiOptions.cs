using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Core.Settings
{
    /// <summary>
    /// The options for chat completions request.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GenAiOptions
    {
        public List<GenAiSettings> Settings { get; set; }
    }

    /// <summary>
    /// The settings for chat completions request.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GenAiSettings
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
        /// The maximum number of retries to allow.
        /// </summary>
        public int MaxRetries { get; set; } = 0;

        /// <summary>
        /// the maximum delay in milliseconds between retries.
        /// </summary>
        public int RetryMinDelay { get; set; } = 1000;

        /// <summary>
        /// the maximum delay in milliseconds between retries.
        /// </summary>
        public int RetryMaxDelay { get; set; } = 10000;

        /// <summary>
        /// if set to true, the delay between retries will grow exponentially,
        /// limited by the values of <see cref="RetryMinDelay"/> and <see cref="RetryMaxDelay"/>. 
        /// Otherwise, the delay will be fixed by the value of <see cref="RetryMinDelay"/>.
        /// </summary>
        public bool ExponentialBackoff { get; set; } = false;
    }
}
