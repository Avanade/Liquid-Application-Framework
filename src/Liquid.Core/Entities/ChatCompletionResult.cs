namespace Liquid.Core.Entities
{
    /// <summary>
    ///  Chat completions result set.
    /// </summary>
    public class ChatCompletionResult
    {
        /// <summary>
        /// The content of the response message.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The reason the model stopped generating tokens, together with any applicable details.
        /// </summary>
        public string FinishReason { get; set; }

        /// <summary>
        /// The total number of tokens processed for the completions request and response.
        /// </summary>
        public int Usage { get; set; }

    }
}
