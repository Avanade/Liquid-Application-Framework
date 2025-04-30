using Liquid.Core.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Core.GenAi.Settings
{
    /// <summary>
    /// The options for chat completions request.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CompletionsOptions
    {
        /// <summary>
        /// Client connection alias to use for a chat completions request.
        /// This connection must be configured in application previously <see cref="GenAiSettings"/>
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// The deployment name to use for a chat completions request. 
        /// </summary>
        public string DeploymentName { get; set; }

        /// <summary>
        /// The deployment name to use for an embeddings request. 
        /// </summary>
        public string EmbeddingModelName { get; set; }

        /// <summary>
        /// Sampling temperature to use that controls the apparent creativity of generated
        /// completions.
        /// </summary>
        public float Temperature { get; set; } = (float)0.7;

        /// <summary>
        /// Gets the maximum number of tokens to generate. 
        /// </summary>
        public int? MaxTokens { get; set; } = null;

        /// <summary>
        ///   An alternative value to <see cref="Temperature"/>, called nucleus sampling, that causes
        ///   the model to consider the results of the tokens with <see cref="TopP"/> probability
        ///   mass.
        /// </summary>
        public float TopP { get; set; } = (float)0.95;

        /// <summary>
        ///  Gets or sets a value that influences the probability of generated tokens appearing based on their
        ///  cumulative frequency in generated text.
        /// </summary>
        public int FrequencyPenalty { get; set; } = 0;

        /// <summary>
        /// Gets or sets a value that influences the probability of generated tokens appearing based on their
        /// existing presence in generated text.
        /// </summary>
        public int PresencePenalty { get; set; } = 0;
    }
}
