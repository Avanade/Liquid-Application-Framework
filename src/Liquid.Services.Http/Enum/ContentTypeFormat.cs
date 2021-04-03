using System;

namespace Liquid.Services.Http.Enum
{
    /// <summary>
    /// Content type format to be send/received in a request.
    /// </summary>
    [Serializable]
    public enum ContentTypeFormat
    {
        /// <summary>
        /// The Json content type format. Equivalent to 'application/json'.
        /// </summary>
        Json,
        /// <summary>
        /// The form data content type format.
        /// </summary>
        FormData,
        /// <summary>
        /// The Multipart FormData content type format. Equivalent to 'multipart/form-data'
        /// </summary>
        MultipartFormData,
        /// <summary>
        /// The Xml content type format. Equivalent to 'application/xml or text/xml'
        /// </summary>
        Xml
    }
}