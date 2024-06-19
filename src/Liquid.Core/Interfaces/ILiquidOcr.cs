using Liquid.Core.Entities;
using System.IO;
using System.Threading.Tasks;

namespace Liquid.Ai.Core.Interfaces
{
    /// <summary>
    /// Service to analyze information from documents and images and extract it into structured data. 
    /// It provides the ability to use prebuilt models to analyze receipts,
    /// business cards, invoices, to extract document content, and more.
    /// </summary>
    public interface ILiquidRecognition
    {
        /// <summary>
        /// Analyzes pages from one or more documents, using a model built with custom documents or one of the prebuilt
        /// models provided by the Form Recognizer service.
        /// </summary>
        /// <param name="doc">The stream containing one or more documents to analyze.</param>
        /// <param name="modelId">
        /// The ID of the model to use for analyzing the input documents. When using a custom built model
        /// for analysis, this parameter must be the ID attributed to the model during its creation. When
        /// using one of the service's prebuilt models, one of the supported prebuilt model IDs must be passed.
        /// </param>
        /// <param name="clientId">The ID of the client configuration to use for analyzing the input documents.
        /// When using a default instace of client, this parameter don't need to be passed, but it's default value
        /// must be configured on application settings.</param>
        Task<OcrResult> AnalyzeDocumentAsync(Stream doc, string clientId = "default" , string? modelId = "prebuilt-layout");

        /// <summary>
        /// Analyzes pages from one or more documents, using a model built with custom documents or one of the prebuilt
        /// models provided by the Form Recognizer service. 
        /// </summary>
        /// <param name="uri">The absolute URI of the remote file to analyze documents from.</param>
        /// <param name="modelId">
        /// The ID of the model to use for analyzing the input documents. When using a custom built model
        /// for analysis, this parameter must be the ID attributed to the model during its creation. When
        /// using one of the service's prebuilt models, one of the supported prebuilt model IDs must be passed.
        /// </param>
        /// <param name="clientId">The ID of the client configuration to use for analyzing the input documents.
        /// When using a default instace of client, this parameter don't need to be passed, but it's default value
        /// must be configured on application settings.</param>
        Task<OcrResult> AnalyzeDocumentFromUriAsync(string uri, string clientId = "default", string? modelId = "prebuilt-layout");

        /// <summary>
        /// Classifies one or more documents using a document classifier built with custom documents.
        /// </summary>
        /// <param name="doc">The ID of the document classifier to use.</param>
        /// <param name="modelId">The stream containing one or more documents to classify.</param>
        /// <param name="clientId">The ID of the client configuration to use for analyzing the input documents.
        /// When using a default instace of client, this parameter don't need to be passed, but it's default value
        /// must be configured on application settings.</param>
        Task<OcrResult> ClassifyDocumenAsync(Stream doc, string clientId = "default", string? modelId = "prebuilt-layout");

        /// <summary>
        /// Classifies one or more documents using a document classifier built with custom documents.
        /// </summary>
        /// <param name="uri">The absolute URI of the remote file to classify documents from.</param>
        /// <param name="modelId">The stream containing one or more documents to classify.</param>
        /// <param name="clientId">The ID of the client configuration to use for analyzing the input documents.
        /// When using a default instace of client, this parameter don't need to be passed, but it's default value
        /// must be configured on application settings.</param>
        Task<OcrResult> ClassifyDocumenFromUriAsync(string uri, string clientId = "default", string? modelId = "prebuilt-layout");
    }
}
