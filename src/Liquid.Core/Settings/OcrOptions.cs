using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Core.Settings
{
    /// <summary>
    /// Set of OCR service settings.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class OcrOptions
    {
        /// <summary>
        /// Collection of OCR service connection settings.
        /// </summary>
        public List<OcrSettings> Settings { get; set; }
    }

    /// <summary>
    /// OCR service connection settings.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class OcrSettings
    {
        /// <summary>
        /// OCR service connection alias.
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// OCR service absolute Uri.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// OCR service access key.
        /// </summary>
        public string Key { get; set; }
    }
}