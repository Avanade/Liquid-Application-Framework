using System.Collections.Generic;
using System.Linq;
namespace Liquid.Core.Extensions
{
    /// <summary>
    /// Util Extensions Class
    /// </summary>
    public static class CommonExtensions
    {
        /// <summary>
        /// The Gzip content type
        /// </summary>
        public const string GZipContentType = "application/gzip";

        /// <summary>
        /// The content type header
        /// </summary>
        public const string ContentTypeHeader = "ContentType";

        /// <summary>
        /// Adds the range from the elements dictionary to the source dictionary. If the element from elements dictionary alreads exists in source, it will be discarded.
        /// </summary>
        /// <param name="source">The source dictionary.</param>
        /// <param name="elements">The elements to be added to source.</param>
        public static void AddRange(this IDictionary<string, object> source, IDictionary<string, object> elements)
        {
            if (elements == null || !elements.Any()) return;

            foreach (var element in elements)
            {
                if (!source.ContainsKey(element.Key)) source.Add(element);
            }
        }
    }
}