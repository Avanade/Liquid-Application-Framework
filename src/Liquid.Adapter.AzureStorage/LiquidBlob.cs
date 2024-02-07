using System.Diagnostics.CodeAnalysis;

namespace Liquid.Adapter.AzureStorage
{
    /// <summary>
    /// Set de propriedades referentes à um item do BlobStorage.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LiquidBlob
    {
        /// <summary>
        /// Lista de tags referentes ao blob.
        /// </summary>
        public IDictionary<string, string>? Tags { get; set; }

        /// <summary>
        /// Conteúdo do blob.
        /// </summary>
        public byte[]? Blob { get; set; }

        /// <summary>
        /// Nome do arquivo no Storage.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Caminho do blob.
        /// </summary>
        public string AbsoluteUri { get; set; }

    }
}
